using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests;

public class GPUTests
{
    [TestCase(GraphicsLevelEnum.Low, 1080, 50000)]
    [TestCase(GraphicsLevelEnum.Low, 1440, 50000)]
    [TestCase(GraphicsLevelEnum.Low, 2000, 50000)]
    [TestCase(GraphicsLevelEnum.Low, 4000, 500000)]
    [TestCase(GraphicsLevelEnum.Medium, 1080, 50000)]
    [TestCase(GraphicsLevelEnum.Medium, 1440, 180000)]
    [TestCase(GraphicsLevelEnum.Medium, 2000, 50000)]
    [TestCase(GraphicsLevelEnum.Medium, 4000, 50000)]
    [TestCase(GraphicsLevelEnum.High, 1080, 50000)]
    [TestCase(GraphicsLevelEnum.High, 1440, 50000)]
    [TestCase(GraphicsLevelEnum.High, 2000, 200000)]
    [TestCase(GraphicsLevelEnum.High, 4000, 10000000)]
    [TestCase(GraphicsLevelEnum.Ultra, 1080, 51000)]
    [TestCase(GraphicsLevelEnum.Ultra, 1440, 120000)]
    [TestCase(GraphicsLevelEnum.Ultra, 2000, 300000)]
    [TestCase(GraphicsLevelEnum.Ultra, 4000, 180000)]

    public async Task ManualTest(GraphicsLevelEnum graphicsLevel, int resolution, int budget)
    {
        var req = new Requirements()
        {
            GraphicsLevel = graphicsLevel,
            ScreenResolution = (ushort)resolution,
            Budget = (uint)budget,
            Rgb = [RgbEnum.GPU]
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
