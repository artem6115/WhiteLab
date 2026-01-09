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
            },
            new Requirements()
            {
                GraphicsLevel = GraphicsLevelEnum.Low,
                ScreenResolution = 1080,
                Budget = 150000,
                ColorStyle = ColorStyleEnum.White,
                Programs = ["Resident_Evil_4_Remake"]
            },
            new Requirements()
            {
                GraphicsLevel = GraphicsLevelEnum.Medium,
                ScreenResolution = 1440,
                Budget = 150000,
                ColorStyle = ColorStyleEnum.White,
                Programs = ["Resident_Evil_4_Remake"]
            },
            new Requirements(){
                GraphicsLevel = GraphicsLevelEnum.Low,
                ScreenResolution = 1080,
                Budget = 130000,
                PowerLevel = PowerLevelEnum.Gaming,
                OverclockingSupport = OverclockingEnum.ALL
            },
            new Requirements(){
                GraphicsLevel = GraphicsLevelEnum.Low,
                ScreenResolution = 1080,
                Budget = 300000,
                PowerLevel = PowerLevelEnum.HardGamingAndRender,
                OverclockingSupport = OverclockingEnum.ALL
            },
            new Requirements(){
                GraphicsLevel = GraphicsLevelEnum.High,
                ScreenResolution = 2000,
                Budget = 300000,
                PowerLevel = PowerLevelEnum.HardGamingAndRender,
                OverclockingSupport = OverclockingEnum.ALL
            },
            new Requirements(){
                GraphicsLevel = GraphicsLevelEnum.Ultra,
                ScreenResolution = 4000,
                Budget = 300000,
                PowerLevel = PowerLevelEnum.HardGamingAndRender,
                OverclockingSupport = OverclockingEnum.ALL
            },
            new Requirements(){
                GraphicsLevel = GraphicsLevelEnum.Medium,
                ScreenResolution = 1440,
                Budget = 900000,
                PowerLevel = PowerLevelEnum.HighGaming,
                OverclockingSupport = OverclockingEnum.Default,
                YangestComponents = false
            },
            ];

            foreach (var r in reqs) yield return r;
        }

        internal static IEnumerable GetReleaseRequirements()
        {
            (Requirements req, List<string> components)[] testCases = 
            [
                
                
            ];


            foreach (var t in testCases) yield return t;
        }
    }
}
