namespace WhiteLab.PCConfigurator.Requirenments;

public class Requirements
{

    public uint Budget { get; set; }
    public byte Region { get; set; }
    //price
    public TargetEnum Target { get; set; }
    public ushort ScreenResolution { get; set; }
    public PowerLevelEnum PowerLevel { get; set; }
    public List<string> Programs { get; set; } = new();
    public ImportanceMultithreadingEnum Multithreading { get; set; }
    public OverclockingEnum OverclockingSupport { get; set; }

    // system requirements

    public bool SplitDisk { get; set; }
    public ushort MemorySize { get; set; }
    public bool PCUpgrade { get; set; }
    public byte PCUpgradeCooldown { get; set; }

    //other
    public FormFactor FormFactor { get; set; }
    public BodyTypeEnum BodyType { get; set; }
    public bool WillUnderTablePosition { get; set; }
    public RgbEnum Rgb { get; set; }
    public ColorStyleEnum ColorStyle { get; set; }
    //front

    public List<WishesEnum> Wishes { get; set; } = new();
}
