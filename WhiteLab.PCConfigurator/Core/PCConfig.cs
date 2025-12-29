namespace WhiteLab.PCConfigurator.Core;

public class PCConfig
{
    public class PCComponent
    {
        public required string Type { get; set; }
        public required string Name { get; set; }
        public string? SimilarModels { get; set; }
        public string? Description { get; set; }
    }

    public List<PCComponent> Components { get; set; } = new();

    public PCComponent? this[string name] => Components.FirstOrDefault
        (c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));
}
