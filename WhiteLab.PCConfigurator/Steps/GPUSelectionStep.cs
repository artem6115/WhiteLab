using System.Linq;
using System.Reflection;
using WhiteLab.PCConfigurator.Components;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Steps;

internal class GPUSelectionStep : IStep
{
    private readonly PCContainer _container;
    private readonly ComponentRelationships _relationships;
    private readonly Requirenments.Requirements _requirements;
    private List<GPU> _setupGpus;
    private List<string> _gpusSorted;

    public GPUSelectionStep(Requirenments.Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _container = container;
        _relationships = relationships;
        _requirements = requirements;
    }

    public int MaxCheapLevel { get => 4; }

    public void SetupBaseComponents()
    {
        _gpusSorted = _relationships.GPUSoftMatrix["sorted_gpu"]!
            .AsArray()
            .Select(m => m!.GetValue<string>().ToLower().Trim())
            .ToList(); //список видеокарт по мощности

        var a = _relationships.GPUTirMatrix.AsArray()
            .Where(o => o?["resolution"]?.GetValue<string>() == _requirements.ScreenResolution.ToString()).ToList()
            .Where(o => o?["settings"]?.GetValue<string>() == _requirements.GraphicsLevel.ToString()).ToList()
            .FirstOrDefault()?["models"]?.AsArray().Select(r => r!.GetValue<string>());
        var b = a
            .Select(GetModelAndRam);
        var c = b
            .Select(GetGPUTirForSoft);
        var tirOptModels = c
            .SelectMany(ApplayAlternativeGPU)
            .ToArray() ?? [];

        var a2 = _requirements.Programs
            .Select(GetNormilizeName)
            .Where(p => p is not null);
        var b2 = a2
            .Select(p =>
                _relationships.GPUSoftMatrix["applications"]
                ?[p!]
                ?["resolutions"]
                ?[_requirements.ScreenResolution.ToString()]
                ?[_requirements.GraphicsLevel.ToString()]);
        var c2 = b2
            .Select(n => $"{n!["recommended_gpu"]!.GetValue<string>()} {n!["vram"]!.GetValue<string>()}");
        var d2 = c2
            .Select(GetModelAndRam);
        var i2 = d2
            .Select(GetGPUTirForSoft);
        var softOptModels = i2.SelectMany(ApplayAlternativeGPU)
            .ToArray() ?? [];
        var powerLvl = Math.Max(tirOptModels.Min(m => m.Power), (softOptModels.Length > 0) ? softOptModels.Max(m => m.Power) : 0);
        int ramLevel = (softOptModels.Length > 0) ? softOptModels.Max(m => m.RAM) : tirOptModels.Min(m => m.RAM);
        var resultModels = softOptModels.Where(m => m.Power >= powerLvl)
                .Concat(tirOptModels.Where(m => m.Power >= powerLvl))
                .Where(m => m.RAM >= ramLevel)
                .ToList();

        var selectedTopModel = _gpusSorted
            .Skip(powerLvl).Where(m => int.Parse(m.Split(' ').Last().Replace("gb","")) > ramLevel)
            .Where(m => !_requirements.YangestComponents || m.StartsWith("rtx 5"))
            .FirstOrDefault();          
        if (selectedTopModel == null)
        {
            _container.Gpus = new();
            return;
        }

        var modelSplit = GetModelAndRam(selectedTopModel);
        resultModels.Add((modelSplit.Model, modelSplit.RAM, _gpusSorted.IndexOf(selectedTopModel)));


        _setupGpus = _container.Gpus.Where(g => resultModels.Where(m => string.Equals(m.Seria, g.Seria, StringComparison.InvariantCultureIgnoreCase) && g.RAM >= m.RAM).Any())
            .Where(WhereFormFactorAndGPUWidth)
            .ToList();
        _container.Gpus = [];
    }

    private string? GetNormilizeName(string alias)
    {
        var programs = _relationships.GPUSoftMatrix["programm_alias"];
        foreach (var program in programs!.AsObject())
        {
            var aliass = program.Value!.AsArray().Select(v => v!.GetValue<string>().ToLower());
            if (aliass.Contains(alias.ToLower().Trim().Replace(' ','_'))) return program.Key;
        }

        return null;
    }

    (string Seria, int RAM ,int Power) GetGPUTirForSoft((string Seria, int RAM) model)
    {
        var gpuFullname = $"{model.Seria} {model.RAM}gb";
        var normalizeGPU = _gpusSorted.FirstOrDefault(m => m == gpuFullname);
        if(normalizeGPU == null)
        {
            var gpusNormal = _gpusSorted
                .Where(m => m.StartsWith(model.Seria))
                .Select(GetModelAndRam)
                .OrderBy(m => m.RAM)
                .FirstOrDefault(m => m.RAM >= model.RAM);
            normalizeGPU = $"{gpusNormal.Model} {gpusNormal.RAM}gb";
        }

        if (normalizeGPU == null) return (model.Seria, model.RAM, -1);

        return (model.Seria, model.RAM, _gpusSorted.IndexOf(normalizeGPU));
    }

    private IEnumerable<(string Seria, int RAM, int Power)> ApplayAlternativeGPU((string Seria, int RAM, int Power) model)
    {
        var gpuFullname = $"{model.Seria} {model.RAM}gb";
        return _relationships.GPUSoftMatrix["gpu_alternative"]![gpuFullname]
            ?.AsArray()
            .Select(mm => mm!.GetValue<string>())
            .Select(GetModelAndRam)
            .Select(m => (Seria: m.Model, m.RAM, model.Power)) ?? [model];
    }


    public bool FilterAndCheap(int cheapLevel = 0)
    {
        _container.Gpus = [];
        IEnumerable<GPU> gpus;
        switch (cheapLevel)
        {
            case 0:
                gpus = _setupGpus
                    .Where(g => !_requirements.YangestComponents || g.Seria.StartsWith("RTX 5"))
                    .Where(WhereColorStyleAndGPUColor)
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.GPULed) || g.Led)
                    .Where(WhereRgbAndSupportGPU);
                break;
            case 1:
               
                _container.GpusInfo += $"Требования к наличию rgb были опущены";
                gpus = _setupGpus
                    .Where(g => !_requirements.YangestComponents || g.Seria.StartsWith("RTX 5"))
                    .Where(WhereColorStyleAndGPUColor)
                    .Where(g => !_requirements.Wishes.Contains(WishesEnum.GPULed) || g.Led);
                break;
            case 2:
                _container.GpusInfo += $"Требования к наличию подсветки и rgb были опущены";
                gpus = _setupGpus
                    .Where(g => !_requirements.YangestComponents || g.Seria.StartsWith("RTX 5"))
                    .Where(WhereColorStyleAndGPUColor);
                break;
            case 3:
                _container.GpusInfo += $"Требования к цвету, наличию подсветки и rgb были опущены";
                gpus = _setupGpus
                    .Where(g => !_requirements.YangestComponents || g.Seria.StartsWith("RTX 5"));
                break;
            default:
                _container.GpusInfo += $"Требования к новизне, цвету, наличию подсветки и rgb были опущены";
                gpus = _setupGpus;
                break;
        }

        _container.Gpus = gpus.OrderBy(g => g.Price).ToList();
        return _container.Gpus.Any() && _container.CalculatePrice() <= _requirements.Budget * 0.55;
    }

    private (string Model, int RAM) GetModelAndRam(string name)
    {
        var ram = name.Substring(name.Length - 4).Trim();
        var model = name.Replace(ram, "").TrimEnd();
        return (model.ToLower(), int.Parse(ram.Replace("GB", "", StringComparison.InvariantCultureIgnoreCase)));
    }

    private bool WhereFormFactorAndGPUWidth(GPU gpu)
    {

        return _requirements.FormFactor switch 
        {
            FormFactor.ATX => gpu.Width <= 380,
            FormFactor.MicroATX => gpu.Width <= 360,
            FormFactor.MiniATX => gpu.Width <= 320,
            _ => true 
        };
    }

    private bool WhereColorStyleAndGPUColor(GPU gpu)
    {
        var color = gpu.Color.ToLower().Trim();
        return _requirements.ColorStyle switch
        {
            ColorStyleEnum.Dark => color == "black" || color == "dark" || color == "чёрный" || color == "черный",
            ColorStyleEnum.Silver => color == "silver" || color == "gray" || color == "серебряный " || color == "серый",
            ColorStyleEnum.White => color == "white" || color == "белый",
            _ => true
        };
    }

    private bool WhereRgbAndSupportGPU(GPU gpu)
    {
        if (!_requirements.Rgb.Contains(RgbEnum.GPU)) return true;
        var result = gpu.Models.Where(m => m.ToLower().Contains("rgb")).ToList();
        if (result.Any())
        {
            gpu.Models = result;
            gpu.ModelsString = string.Join(',', gpu.Models);
            return true;
        }

        return false;
    }

    public void ClearContainer()
    {
        _container.Gpus = [];
        _container.GpusInfo = "";
    }
}
