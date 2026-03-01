namespace WhiteLab.PCConfigurator.Components;

public class RAM : IComponent
{
    public string Type => "Оперативная память";
    public string Name {  get; set; }
    public int Price {  get; set; }
    public string RAMType { get; set; }
    public int Capacity { get; set; }
    public int Frequency { get; set; }
    public bool Xmp { get; set; }
    public bool Expo { get; set; }
    public string Timings { get; set; }
    public string Color { get; set; }
    public bool Rgb { get; set; }

}
