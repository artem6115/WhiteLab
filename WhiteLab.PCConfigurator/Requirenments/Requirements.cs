namespace WhiteLab.PCConfigurator.Requirenments;

public class Requirements
{

    public uint Budget { get; set; }
    public byte Region { get; set; }
    //price
    //public TargetEnum Target { get; set; }
    public ushort ScreenResolution { get; set; }
    public GraphicsLevelEnum GraphicsLevel { get; set; }

    public PowerLevelEnum PowerLevel { get; set; } = new();
    public List<string> Programs { get; set; } = new();

    public ImportanceMultithreadingEnum Multithreading { get; set; }
    public OverclockingEnum OverclockingSupport { get; set; }

    // system requirements

    public bool SplitDisk { get; set; }
    public ushort MemorySize { get; set; }
    public bool PCUpgrade { get; set; }
    public byte PCUpgradeCooldown { get; set; }

    //other
    public bool YangestComponents { get; set; }
    public FormFactor FormFactor { get; set; }
    public BodyTypeEnum BodyType { get; set; }
    public bool WillUnderTablePosition { get; set; }
    public List<RgbEnum> Rgb { get; set; } = new();
    public ColorStyleEnum ColorStyle { get; set; }
    //front

    public List<WishesEnum> Wishes { get; set; } = new();
}
