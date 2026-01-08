namespace WhiteLab.PCConfigurator.Requirenments;

public class Requirements
{

    public uint Budget { get; set; }
    //price
    public ushort ScreenResolution { get; set; } = 1080;
    public GraphicsLevelEnum GraphicsLevel { get; set; }
    public bool ExcludeGpu { get; set; }
    public PowerLevelEnum PowerLevel { get; set; } = new();
    public List<string> Programs { get; set; } = new();
    public OverclockingEnum OverclockingSupport { get; set; }

    // system requirements

    public bool SplitDisk { get; set; }
    public ushort MemorySize { get; set; }

    //other
    public bool YangestComponents { get; set; }
    public FormFactor FormFactor { get; set; }
    public BodyTypeEnum BodyType { get; set; }
    public List<RgbEnum> Rgb { get; set; } = new();
    public ColorStyleEnum ColorStyle { get; set; }
    //front

    public List<WishesEnum> Wishes { get; set; } = new();
}
