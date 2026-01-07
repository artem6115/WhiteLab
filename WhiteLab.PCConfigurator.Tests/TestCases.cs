using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests
{
    internal static class TestCases
    {
        public static IEnumerable GetRequirements()
        {
            Requirements[] reqs =
            [
                new Requirements()
            {
                GraphicsLevel = GraphicsLevelEnum.Low,
                ScreenResolution = 1080,
                Budget = 120000,
                Rgb = [RgbEnum.GPU],
                YangestComponents = true,
                ColorStyle = ColorStyleEnum.White
            },
            new Requirements()
            {
                GraphicsLevel = GraphicsLevelEnum.Low,
                ScreenResolution = 1080,
                Budget = 120000,
                Rgb = [RgbEnum.GPU],
                YangestComponents = true,
                ColorStyle = ColorStyleEnum.White
            }
            ];

            foreach (var r in reqs) yield return r;
        }
    }
}
