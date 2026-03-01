namespace WhiteLab.PCConfigurator.Components;

public class Cooling : IComponent
{
    public string Type => "Охлаждение";
    public string Name {  get; set; }
    public int Price {  get; set; }
    public int PipeCount { get; set; }
    public int PipeDiametr { get; set; }
    public int FanCount { get; set; }
    public int TDP { get; set; }
    public bool Rgb { get; set; }
    public string Color { get; set; }
    public string FreezingType { get; set; }
}
