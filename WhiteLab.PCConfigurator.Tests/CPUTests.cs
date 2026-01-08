using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests;

public class CPUTests
{
    [TestCase(PowerLevelEnum.Office, 50000)]
    [TestCase(PowerLevelEnum.School, 150000)]
    [TestCase(PowerLevelEnum.Study, 150000)]
    [TestCase(PowerLevelEnum.Streamin, 50000)]
    [TestCase(PowerLevelEnum.Gaming, 50000)]
    [TestCase(PowerLevelEnum.HighGaming, 50000)]
    [TestCase(PowerLevelEnum.ProGaming, 50000)]
    [TestCase(PowerLevelEnum.Render, 50000)]
    [TestCase(PowerLevelEnum.HighGaming, 50000)]
    [TestCase(PowerLevelEnum.ProRender, 50000)]
    [TestCase(PowerLevelEnum.HardGamingAndRender, 300000)]
    [TestCase(PowerLevelEnum.ProGamingAndRender, 50000)]
    public async Task ManualTest(PowerLevelEnum powerLevel,  int budget)
    {
        var req = new Requirements()
        {
            PowerLevel = powerLevel,
            Budget = (uint)budget,
            ExcludeGpu = false
            //YangestComponents = true
            //ColorStyle = ColorStyleEnum.White

        };
        //req.Wishes.Add(WishesEnum.GPULed);
        req.Programs.AddRange(["кс"]);
        var pc = new Core.PCConfigurator();
        var cnf = await pc.Configure(req, default);

        Assert.That(cnf, Is.Not.Null);
        if(!cnf.IsSuccess)
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

    //test RTX 3070 find alternative gpu
}
