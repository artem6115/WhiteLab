using WhiteLab.PCConfigurator.Core;

namespace WhiteLab.PCConfigurator.Steps;

internal interface IStep
{
    void SetupBaseComponents();
    bool FilterAndCheap(int cheapLevel = 0);
    public int MaxCheapLevel { get; }

}
