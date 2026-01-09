using System.Collections;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests;

internal class ReleaseTests
{
    [TestCaseSource(nameof(GetRequirements), Category = "ReleaseTests")]
    public async Task MainTestAsync((Requirements req, List<string> components) testCase)
    {
        
    }

    public static IEnumerable GetRequirements() => TestCases.GetReleaseRequirements();
}
