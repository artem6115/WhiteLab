namespace WhiteLab.PCConfigurator.Components;

public class Power : IComponent
{
    public string Type => "Блок питания";

    public string Name {  get; set; }

    public int Price {  get; set; }
}
