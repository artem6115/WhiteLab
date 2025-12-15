using WhiteLab.PCConfigurator.ModelProvider.Models;

namespace WhiteLab.Models.ModelProvider;

public interface IModelProvider
{
    Task<List<CPU>> GetCPUs(CancellationToken ct);
    Task<List<GPU>> GetGPUs(CancellationToken ct);
    Task<List<RAM>> GetRAMs(CancellationToken ct);
    Task<List<SSD>> GetSSDs(CancellationToken ct);
    Task<List<HDD>> GetHDDs(CancellationToken ct);
    Task<List<MatherBoard>> GetMotherBoards(CancellationToken ct);
    Task<List<Cooling>> GetCoolings(CancellationToken ct);
    Task<List<Fan>> GetFans(CancellationToken ct);
    Task<List<Body>> GetBodies(CancellationToken ct);
    Task<List<Power>> GetPowers(CancellationToken ct);
}
