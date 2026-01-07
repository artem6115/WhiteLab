using WhiteLab.PCConfigurator.Core;

namespace WhiteLab.PCConfigurator.Steps;

internal interface IStep
{
    void SetupBaseComponents();
    bool FilterAndCheap(int cheapLevel = 0);
    void ClearContainer();
    public int MaxCheapLevel { get; }

}
