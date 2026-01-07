
using System.Collections;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests;

internal class ComposeTests
{
    [TestCaseSource(nameof(GetRequirements))]
    public async Task ManualTestAsync(Requirements requirements)
    {
    }

    public static IEnumerable GetRequirements() => TestCases.GetRequirements();
}
