namespace WhiteLab.PCConfigurator.Core;

public class PCConfig
{
    public class PCComponent
    {
        public PCComponent(string type, string name, int price, string? similarModels, string? description)
        {
            Type = type;
            Name = name;
            Price = price;
            SimilarModels = similarModels;
            Description = description;
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
