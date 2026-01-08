using WhiteLab.PCConfigurator.Components;

namespace WhiteLab.PCConfigurator.Core;

internal class PCContainer
{
    public List<GPU> Gpus { get; set; }
    public string GpusInfo { get; internal set; }
    public List<CPU> Cpus { get; set; }
    public string CpusInfo { get; internal set; }
    public List<Motherboard> Motherboards { get; set; }
    public string MotherboardInfo { get; internal set; }


    internal int CalculatePrice() => (Gpus?.FirstOrDefault()?.Price ?? 0) +
        (Cpus?.FirstOrDefault()?.Price ?? 0) +
        (Motherboards?.FirstOrDefault()?.Price ?? 0);

    internal bool IsComposed(bool excludeGPU = false) => (Gpus.Any() || excludeGPU) && Cpus.Any() && Motherboards.Any();
}
