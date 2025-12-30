using WhiteLab.PCConfigurator.Core;

namespace WhiteLab.PCConfigurator.Steps;

internal interface IStep
{
    PCContainer SelectComponents();
    PCContainer GetCheaper(int cheapLevel);
}
