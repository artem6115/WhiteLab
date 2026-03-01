using System;
using System.Collections.Generic;
using System.Text;
using WhiteLab.PCConfigurator.Components;
using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Steps;

internal class RAMSelectionStep : IStep
{
    private readonly Requirements _requirements;
    private readonly PCContainer _container;
    private readonly ComponentRelationships _relationships;
    private List<RAM> _setupRAMs;
    private int _tir;
    private string _stepsCount;

    public RAMSelectionStep(Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _requirements = requirements;
        _container = container;
        _relationships = relationships;
    }

    public int MaxCheapLevel => throw new NotImplementedException();

    public void ClearContainer()
    {
        throw new NotImplementedException();
    }

    public bool FilterAndCheap(int cheapLevel = 0)
    {
        throw new NotImplementedException();
    }

    public void SetupBaseComponents()
    {
        throw new NotImplementedException();
    }
}
