using System;
using System.Collections.Generic;
using System.Text;
using WhiteLab.PCConfigurator.Components;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Steps;

internal class RAMSelectionStep : IStep
{
    private readonly Requirements _requirements;
    private readonly PCContainer _container;
    private readonly ComponentRelationships _relationships;
    private List<RAM> _setupRAMs;
    private int _frq;
    private int _cap;

    public RAMSelectionStep(Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _requirements = requirements;
        _container = container;
        _relationships = relationships;
    }

    public int MaxCheapLevel => 2;

    public void ClearContainer()
    {
        _container.RAMInfo = "";
        _container.RAMs = [];
    }

    public bool FilterAndCheap(int cheapLevel = 0)
    {
        IEnumerable<RAM> rams = _setupRAMs;
        var cap = 0;
        switch (cheapLevel)
        {
            case 0:
                var frq = (_requirements.GraphicsLevel == GraphicsLevelEnum.Ultra) ? 800 :
                    (_requirements.GraphicsLevel == GraphicsLevelEnum.High) ? 400 : 0;
                cap = (_requirements.ScreenResolution == 4000) ? 32 :
                    (_requirements.ScreenResolution == 2000) ? 16 : 0;
                rams = _setupRAMs.Where(m => m.Frequency >= _frq + frq)
                    .Where(m => m.Capacity * 4 >= _cap + cap);
                break;
            case 1:
                cap = (_requirements.ScreenResolution == 4000) ? 32 :
                    (_requirements.ScreenResolution == 2000) ? 16 : 0;
                rams = _setupRAMs.Where(m => m.Capacity * 4 >= _cap + cap);
                break;
        }
        rams = rams.Select(r =>
        {
            var k = ((cap + _cap) / r.Capacity);
            r.Capacity *= k;
            r.Price *= k;
            r.ModulsCount = k;
            return r;
        });
        _container.RAMs = rams.OrderByDescending(r => r.Frequency).ThenBy(s => s.Timings).ThenBy(s => s.Price).ToList();

        return _container.RAMs.Any();
    }

    private string? GetNormilizeName(string alias)
    {
        var programs = _relationships.GPUSoftMatrix["programm_alias"];
        foreach (var program in programs!.AsObject())
        {
            var aliass = program.Value!.AsArray().Select(v => v!.GetValue<string>().ToLower());
            if (aliass.Contains(alias.ToLower().Trim().Replace(' ', '_'))) return program.Key;
        }

        return null;
    }

    public void SetupBaseComponents()
    {
        var ddr = _container.Motherboards.First().RAMType;

        var node = _relationships.RAMTirMatrix!
            [_requirements.ScreenResolution.ToString()]!
            [_requirements.GraphicsLevel.ToString().ToLower()]!;

        var tierModels = (speed: node[ddr == "DDR" ? "ddr4_mhz" : "ddr5_mhz"]!.GetValue<int>(),
            timings: node[ddr == "DDR" ? "timings_ddr4" : "timings_ddr4"]!.GetValue<string>().Split('-').Select(t => int.Parse(t)).Max(),
            size: node["ram_size_gb"]!.GetValue<int>());

        var softOptModels = _requirements.Programs
            .Select(GetNormilizeName)
            .Where(p => p is not null)
            .Select(p => _relationships.RAMSoftMatrix[p!]!)
            .Where(p => p is not null)
            .Select(p =>
                (size: p["ram_size"]!.GetValue<int>(),
                speed: p[(ddr == "DDDR4" ? "ram_speed_ddr4" : "ram_speed_ddr5")]!.GetValue<int>(),
                timings: p[(ddr == "DDDR4" ? "timings_ddr4" : "timings_ddr5")]!.GetValue<string>()))
            .ToList() ?? [];
        var frq = Math.Max(tierModels.speed, softOptModels.Max(s => s.speed));
        var timingsSum = Math.Max(tierModels.timings, softOptModels.Max(s => s.timings.Split('-').Select(t => int.Parse(t)).Sum()));

        var rams = _container.RAMs
            .Where(r => r.RAMType == ddr)
            .Where(r => r.Frequency >= frq)
            .Where(r => r.Timings.Split('-').Select(t => int.Parse(t)).Sum() >= timingsSum)
            .ToList();

        _setupRAMs = rams;
        _container.RAMs = [];
    }
}
