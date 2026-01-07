
using WhiteLab.PCConfigurator.Components;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Steps;

internal class CPUSelectionStep : IStep
{
    private readonly Requirements _requirements;
    private readonly PCContainer _container;
    private readonly ComponentRelationships _relationships;
    private List<CPU> _setupCpus;
    private int _tir;

    public CPUSelectionStep(Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _requirements = requirements;
        _container = container;
        _relationships = relationships;
    }

    public int MaxCheapLevel => 3;

    public void SetupBaseComponents()
    {
        var taskTir = _relationships.CPUSoftMatrix["user_work_tiers"]![_requirements.PowerLevel.ToString()]?.GetValue<int>() ?? 0;
        var softTir = _requirements.Programs
            .Select(GetNormilizeName)
            .Where(p => p is not null)
            .Select(p => _relationships.CPUSoftMatrix["programs"]![p!]?.GetValue<int>())
            .Max() ?? 0;
        var gpu = (!_requirements.ExcludeGpu) ? _container.Gpus.FirstOrDefault() : null;
        var gpuSeria = gpu?.Seria ?? "";
        var gpuRam = gpu?.RAM ?? 0;
        var gpuTir = _relationships.GPUSoftMatrix["gpu_requirenment_cputir"]![$"{gpuSeria} {gpuRam}GB"]?.GetValue<int>() ?? 0;
        _tir = Math.Max(Math.Max(taskTir, softTir), gpuTir);
        _setupCpus = _container.Cpus
            .Where(c => c.Tir >= _tir)
            .Where(c => gpu != null || !c.Model.Contains('F'))
            .ToList();
        _container.Cpus = [];
    }

    public bool FilterAndCheap(int cheapLevel = 0)
    {
        _container.Cpus = [];
        IEnumerable<CPU> cpus;
        switch (cheapLevel)
        {
            case 0:
                cpus = _setupCpus
                    .Where(g => !_requirements.YangestComponents || g.Socket == "AM5" || g.Socket == "LGA1851")
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.AM5) || g.Socket == "AM5")
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.LGA1851) || g.Socket == "LGA1851")
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.LGA1700) || g.Socket == "LGA1700")
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.CPUAMD) || g.Socket.StartsWith("AM"))
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.CPUIntel) || g.Socket.StartsWith("LGA"))
                    .Where(g => _requirements.OverclockingSupport != OverclockingEnum.No || OverclockingSupport(g.Model) == false)
                    .Where(g => _requirements.OverclockingSupport != OverclockingEnum.Yes || OverclockingSupport(g.Model) == true);
                break;
            case 1:
                cpus = _setupCpus
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.LGA1700) || g.Socket == "LGA1700")
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.CPUAMD) || g.Socket.StartsWith("AM"))
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.CPUIntel) || g.Socket.StartsWith("LGA"))
                    .Where(g => _requirements.OverclockingSupport != OverclockingEnum.No || OverclockingSupport(g.Model) == false)
                    .Where(g => _requirements.OverclockingSupport != OverclockingEnum.Yes || OverclockingSupport(g.Model) == true);
                break;
            case 2:
                cpus = _setupCpus
                    .Where(g => _requirements.OverclockingSupport != OverclockingEnum.No || OverclockingSupport(g.Model) == false)
                    .Where(g => _requirements.OverclockingSupport != OverclockingEnum.Yes || OverclockingSupport(g.Model) == true);
                break;
            default:
                cpus = _setupCpus;
                break;
        }

        _container.Cpus = cpus.OrderBy(g => g.Price).ToList();
        return _container.Cpus.Any() && _container.CalculatePrice() <= _requirements.Budget;

    }

    internal static bool OverclockingSupport(string model) => model.StartsWith("AMD") || model.Contains('K');

    public void ClearContainer()
    { 
        _container.Cpus = [];
        _container.CpusInfo = "";
    }

    private string? GetNormilizeName(string alias)
    {
        var programs = _relationships.GPUSoftMatrix["programm_alias"];
        foreach (var program in programs!.AsObject())
        {
            var aliass = program.Value!.AsArray().Select(v => v!.GetValue<string>());
            if (aliass.Contains(alias)) return program.Key;
        }

        return null;
    }
}
