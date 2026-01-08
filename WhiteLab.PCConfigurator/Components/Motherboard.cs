using WhiteLab.PCConfigurator.Requirenments;
using WhiteLab.PCConfigurator.Steps;

namespace WhiteLab.PCConfigurator.Components;

public class Motherboard : IComponent
{
    private static string[] _overclocking_ram = ["B760", "B860", "Z790", "Z890", "A520", "A620", "B550", "B650", "X670", "B850"];
    private static string[] _overclocking_cpu = ["Z790", "Z890", "B550", "B650", "X670", "B850"];
    public string Type => "Материнская плата";
    public string Name => Model;
    public string Id { get; set; }
    public string Model { get; set; }
    public string Socket { get; set; }
    public string Chipset { get; set; }
    public int VrmTier { get; set; }
    public string RAMType { get; set; }
    public float PCIVersion { get; set; }
    public string FormFactor { get; set; }
    public string Color { get; set; }
    public bool Rgb { get; set; }
    public int Price { get; set; }

    public static int GetVramTirForTDP(int cpuTdp)
    {
        if (cpuTdp < 81) return 1;
        if (cpuTdp < 131) return 2;
        if (cpuTdp < 181) return 3;
        if (cpuTdp < 201) return 4;
        return 5;
    }

    public override string ToString()
    {
        var n = Environment.NewLine;
        var overclokingCPU = IsCPUOverclockingSupport(this) ? "имеет" : "не имеет";
        var overclokingRAM = IsRAMOverclockingSupport(this) ? "имеет" : "не имеет";
        var rgb = Rgb ? "имеет" : "не имеет";
        return $"Материнская плата на чипсете {Chipset}, с сокетом {Socket}{n}Имеет тип памяти {RAMType}, {overclokingCPU} поддержку разгона CPU{n}{overclokingRAM} поддержку разгона RAM,{n}PCI - {PCIVersion}, FormFactor - {FormFactor}{n}Цвет {Color}, {rgb} rgb подсветку";
    }

    public static bool IsCPUOverclockingSupport(string chipset) => _overclocking_cpu.Contains(chipset.ToUpper());
    public static bool IsRAMOverclockingSupport(string chipset) => _overclocking_ram.Contains(chipset.ToUpper());
    public static bool IsCPUOverclockingSupport(Motherboard board) => IsCPUOverclockingSupport(board.Chipset);
    public static bool IsRAMOverclockingSupport(Motherboard board) => IsRAMOverclockingSupport(board.Chipset);
}
