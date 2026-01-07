namespace WhiteLab.PCConfigurator.Core;

internal static class PCGlobalContainer
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static PCContainer? _container;
    private static ComponentRelationships? _relationships;

    private static DateTime _cacheExpire;

    public async static ValueTask<PCContainer> GetContainerAsync(CancellationToken ct)
    {
        if (_container == null || _cacheExpire > DateTime.Now)
        {
            await RefreshContainerAsync(ct);
        }

        return _container!;
    }

    public async static ValueTask<ComponentRelationships> GetRelationShipsAsync(CancellationToken ct)
    {
        if (_relationships == null || _cacheExpire > DateTime.Now)
        {
            await RefreshContainerAsync(ct);
        }

        return _relationships!;
    }

    private async static ValueTask RefreshContainerAsync(CancellationToken ct)
    {
        try
        {
            await _semaphore.WaitAsync(ct);
            if (_relationships != null && _container != null && _cacheExpire <= DateTime.Now)
            {
                return;
            }

            _cacheExpire = DateTime.Now.AddMinutes(5);//.AddDays(1);
            var con = new PCContainer();
            con.Gpus = await ModelProvider.GetGPUs(ct);
            con.Cpus = await ModelProvider.GetCPUs(ct);

            var rel = new ComponentRelationships();
            rel.GPUSoftMatrix = await ModelProvider.GetGPUSoftMatrix(ct);
            rel.GPUTirMatrix = await ModelProvider.GetGPUTirMatrix(ct);

            rel.CPUSoftMatrix= await ModelProvider.GetCPUSoftMatrix(ct);

            _container = con;
            _relationships = rel;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
