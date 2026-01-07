namespace WhiteLab.PCConfigurator.Components;

public interface IComponent
{
    public string Type { get; }
    public string Name { get; }
    public int Price { get; }
}
