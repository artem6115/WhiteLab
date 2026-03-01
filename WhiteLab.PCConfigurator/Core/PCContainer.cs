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
    public List<RAM> RAMs { get; set; }
    public string RAMInfo { get; internal set; }
    public List<SSD> SSDs { get; set; }
    public string SSDInfo { get; internal set; }
    public List<Cooling> Coolings { get; set; }
    public string CoolongInfo { get; internal set; }
    public List<Power> Powers { get; set; }
    public string PowerInfo { get; internal set; }

    internal int CalculatePrice() => (Gpus?.FirstOrDefault()?.Price ?? 0) +
        (Cpus?.FirstOrDefault()?.Price ?? 0) +
        (Motherboards?.FirstOrDefault()?.Price ?? 0) +
        (RAMs?.FirstOrDefault()?.Price ?? 0) +
        (SSDs?.FirstOrDefault()?.Price ?? 0) +
        (Coolings?.FirstOrDefault()?.Price ?? 0);
    //(Powers?.FirstOrDefault()?.Price ?? 0);

    internal bool IsComposed(bool excludeGPU = false) =>
        (Gpus.Any() || excludeGPU) && Cpus.Any() && Motherboards.Any() && RAMs.Any() && SSDs.Any() && Coolings.Any(); // && Powers.Any();
}
