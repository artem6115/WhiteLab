using WhiteLab.PCConfigurator.Components;

namespace WhiteLab.PCConfigurator.Core;

internal class PCContainer
{
    public List<GPU> Gpus { get; set; }
    public string GpusInfo { get; internal set; }

    internal int CalculatePrice()
    {
        var gpu = Gpus[0].Price;

        return gpu;
    }

    internal bool IsComposed()
    {
        return Gpus.Any();
    }
}
