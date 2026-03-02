using System.Text.Json.Nodes;

namespace WhiteLab.PCConfigurator.Core;

internal class ComponentRelationships
{
    public JsonNode GPUSoftMatrix { get; set; } = null!;
    public JsonNode GPUTirMatrix { get; set; } = null!;
    public JsonNode CPUSoftMatrix { get; set; } = null!;
    public JsonNode RAMSoftMatrix { get; set; } = null!;
    public JsonNode RAMTirMatrix { get; set; } = null!;


}
