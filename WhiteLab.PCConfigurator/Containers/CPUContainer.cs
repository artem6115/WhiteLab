using System;
using System.Collections.Generic;
using System.Text;
using WhiteLab.PCConfigurator.ModelProvider.Models;
using WhiteLab.PCConfigurator.Models;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Containers;

internal class CPUContainer
{
    private readonly Requirements _requirements;
    private readonly List<CPU> _cpus;

    public CPUContainer(List<CPU> cpus, Requirements requirements)
    {
        _requirements = requirements;
        _cpus = cpus;
    }

    public List<CPU> GetOptimized(int relent = 0)
    {    
        return _cpus;
    }


}
