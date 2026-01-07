
using WhiteLab.PCConfigurator.Components;

namespace WhiteLab.PCConfigurator.Core;

public class PCConfig
{
    public class PCComponent
    {
        public PCComponent(IComponent component, string? similarModels, string? description)
        {
            Type = component.Type;
            Name = component.Name;
            Price = component.Price;
            SimilarModels = similarModels;
            Description = component + Environment.NewLine + description;
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string? SimilarModels { get; set; }
        public string? Description { get; set; }
    }

    public List<PCComponent> Components { get; set; } = new();

    public int Price { get => Components.Sum(c => c.Price); }

    public PCComponent? this[string name] => Components.FirstOrDefault
        (c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));
}
