
using System.Collections;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests;

internal class ComposeTests
{
    [TestCaseSource(nameof(GetRequirements))]
    public async Task ManualTestAsync(Requirements req)
    {
        var pc = new Core.PCConfigurator();
        var cnf = await pc.Configure(req, default);

        Assert.That(cnf, Is.Not.Null);
        if (!cnf.IsSuccess)
        {
            Console.WriteLine(cnf.ConfigError!.Message);
            return;
        }

        Console.WriteLine("Total Price: " + cnf.Config!.Price);
        foreach (var component in cnf.Config!.Components)
        {
            Console.WriteLine($"{component.Type}: {component.Name}");
            Console.WriteLine($"{component.SimilarModels}");
            Console.WriteLine($"{component.Description}");
        }
    }

    public static IEnumerable GetRequirements() => TestCases.GetRequirements();
}
