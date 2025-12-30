using WhiteLab.PCConfigurator.Core;

namespace WhiteLab.PCConfigurator.Steps;

internal class GPUSelectionStep : IStep
{
    private readonly PCContainer _container;
    private readonly ComponentRelationships _relationships;
    private readonly Requirenments.Requirements _requirements;

    public GPUSelectionStep(Requirenments.Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _container = container;
        _relationships = relationships;
        _requirements = requirements;
    }

    public PCContainer SelectComponents()
    {
        throw new NotImplementedException();
    }

    public PCContainer GetCheaper(int cheapLevel)
    {
        throw new NotImplementedException();
    }
}
