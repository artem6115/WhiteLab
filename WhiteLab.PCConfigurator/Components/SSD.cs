namespace WhiteLab.PCConfigurator.Components;

public class SSD : IComponent
{
    public string Type => "Диск";

    public string Name { get; set; }

    public int Price {  get; set; }
    public int Capacity { get; set; }
    public string Interface { get; set; }
    public string FormFactor { get; set; }
    public string Color { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }
}
