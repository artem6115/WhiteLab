using System.Text.Json;
using System.Text.Json.Nodes;
using WhiteLab.PCConfigurator.Components;

namespace WhiteLab.PCConfigurator.Core;

internal static class ModelProvider
{
    public async static Task<List<GPU>> GetGPUs(CancellationToken ct)
    {
        var json = await File.ReadAllTextAsync(Path.Combine("ComponentsData", "GPUs.json"), ct);
        return JsonSerializer.Deserialize<List<GPU>>(json)
            ?? throw new InvalidDataException("GPUs.json is invalid or empty");
    }

    public async static Task<JsonNode> GetGPUSoftMatrix(CancellationToken ct)
    {
        var jsonStr = await File.ReadAllTextAsync(Path.Combine("ComponentsData", "GPUSoftMatrix.json"), ct);
        var json = JsonObject.Parse(jsonStr, new JsonNodeOptions { PropertyNameCaseInsensitive = true })
            .ThrowIfDataIsNull("GPUSoftMatrix.json is empty or invalid");
        return json["gpu_recommendation_system_v2"].ThrowIfDataIsNull("GPUSoftMatrix.json is empty or invalid");
    }

    public async static Task<List<CPU>> GetCPUs(CancellationToken ct)
    {
        throw new NotImplementedException();

    }

    public async static Task<List<RAM>> GetRAMs(CancellationToken ct)
    {
        throw new NotImplementedException();

    }

    public async static Task<List<SSD>> GetSSDs(CancellationToken ct)
    {

        throw new NotImplementedException();
    }

    public async static Task<List<HDD>> GetHDDs(CancellationToken ct)
    {
        throw new NotImplementedException();

    }

    public async static Task<List<MatherBoard>> GetMotherBoards(CancellationToken ct)
    {
        throw new NotImplementedException();

    }

    public async static Task<List<Cooling>> GetCoolings(CancellationToken ct)
    {
        throw new NotImplementedException();

    }

    public async static Task<List<Fan>> GetFans(CancellationToken ct)
    {
        throw new NotImplementedException();

    }

    public async static Task<List<Body>> GetBodies(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async static Task<List<Power>> GetPowers(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

}