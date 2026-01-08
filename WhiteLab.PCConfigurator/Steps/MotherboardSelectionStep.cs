using WhiteLab.PCConfigurator.Components;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Steps;

internal class MotherboardSelectionStep : IStep
{
    private Requirements _requirements;
    private PCContainer _container;
    private ComponentRelationships _relationships;
    private List<Motherboard> _motherboards;

    public MotherboardSelectionStep(Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _requirements = requirements;
        _container = container;
        _relationships = relationships;
    }

    public int MaxCheapLevel => 5;

    public void SetupBaseComponents()
    {
        _motherboards = _container.Motherboards;
        _container.Motherboards = [];
    }

    public bool FilterAndCheap(int cheapLevel = 0)
    {
        _container.Motherboards = [];
        IEnumerable<Motherboard> motherboards = _motherboards
            .Where(m => string.Equals(m.Socket, _container.Cpus[0].Socket, StringComparison.InvariantCultureIgnoreCase))
            .Where(m => m.VrmTier >= Motherboard.GetVramTirForTDP(_container.Cpus[0].TDP));
        switch (cheapLevel)
        {
            case 0:
                motherboards = motherboards
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.CPU || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsCPUOverclockingSupport(m) == true)
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.RAM || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsRAMOverclockingSupport(m) == true)
                    .Where(m => _requirements.FormFactor == FormFactor.Default || string.Equals(_requirements.FormFactor.ToString(),m.FormFactor, StringComparison.InvariantCultureIgnoreCase))
                    .Where(WhereColorStyleAndMotherColor)
                    .Where(m => !_requirements.Rgb.Contains(RgbEnum.MotherBoard) || m.Rgb)
                    .Where(m => (_container.Gpus.FirstOrDefault()?.RAM ?? 0) < 12 || m.PCIVersion >= 4);
                break;
            case 1:
                motherboards = motherboards
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.CPU || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsCPUOverclockingSupport(m) == true)
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.RAM || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsRAMOverclockingSupport(m) == true)
                    .Where(m => _requirements.FormFactor == FormFactor.Default || string.Equals(_requirements.FormFactor.ToString(), m.FormFactor, StringComparison.InvariantCultureIgnoreCase))
                    .Where(WhereColorStyleAndMotherColor)
                    .Where(m => (_container.Gpus.FirstOrDefault()?.RAM ?? 0) < 12 || m.PCIVersion >= 4);
                break;
            case 2:
                motherboards = motherboards
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.CPU || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsCPUOverclockingSupport(m) == true)
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.RAM || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsRAMOverclockingSupport(m) == true)
                    .Where(m => _requirements.FormFactor == FormFactor.Default || string.Equals(_requirements.FormFactor.ToString(), m.FormFactor, StringComparison.InvariantCultureIgnoreCase))
                    .Where(m => (_container.Gpus.FirstOrDefault()?.RAM ?? 0) < 12 || m.PCIVersion >= 4);
                break;
            case 3:
                motherboards = motherboards
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.CPU || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsCPUOverclockingSupport(m) == true)
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.RAM || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsRAMOverclockingSupport(m) == true);
                break;
            case 4:
                motherboards = motherboards
                    .Where(m => !(_requirements.OverclockingSupport == OverclockingEnum.RAM || _requirements.OverclockingSupport == OverclockingEnum.ALL) || Motherboard.IsRAMOverclockingSupport(m) == true);
                break;
        }

        _container.Motherboards = motherboards.OrderBy(m => m.Price).ToList();
        return _container.Motherboards.Any() && _container.CalculatePrice() <= _requirements.Budget;

    }

    public void ClearContainer()
    {
        _container.MotherboardInfo = "";
        _container.Motherboards = [];
    }

    private bool WhereColorStyleAndMotherColor(Motherboard m)
    {
        var color = m.Color.ToLower().Trim();
        return _requirements.ColorStyle switch
        {
            ColorStyleEnum.Dark => color == "black" || color == "dark" || color == "чёрный" || color == "черный",
            ColorStyleEnum.Silver => color == "silver" || color == "gray" || color == "серебряный " || color == "серый",
            ColorStyleEnum.White => color == "white" || color == "белый",
            _ => true
        };
    }
}
