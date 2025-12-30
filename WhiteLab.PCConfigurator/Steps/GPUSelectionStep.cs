using WhiteLab.PCConfigurator.Core;

namespace WhiteLab.PCConfigurator.Steps;

internal class GPUSelectionStep : IStep
{
    private readonly PCContainer _container;
    private readonly ComponentRelationships _relationships;
    private readonly Requirenments.Requirements _requirements;

    public GPUSelectionStep(Requirenments.Requirements requirements, PCContainer container, ComponentRelationships relationships)
    {
        _container = container;
        _relationships = relationships;
        _requirements = requirements;
    }

    public PCContainer SelectComponents()
    {
        /*Target 
            1.ScreenResolution
            
            1.PowerLevel
            2.Programs 
            Budget 
            Region 
             
            PCUpgrade
            PCUpgradeCooldown
            FormFactor 
            Rgb 
            ColorStyle
            Wishes
        */
        string[] tirOptModels = _relationships.GPUTirMatrix[_requirements.PowerLevel.ToString()]
            ?[_requirements.ScreenResolution]
            ?.GetValue<string>()
            .Split(',')
            ?? [];
        var softOptModels = _requirements.Programs
            .Select(p => p.Trim().Replace(' ', '_'))
            .Select(p => _relationships.GPUSoftMatrix["applications"]?[p]?["resolutions"]?[_requirements.ScreenResolution.ToString()]?[]);  //Указывал что бы JsonNode игнорировал case, но надо проверить
    }

    public PCContainer GetCheaper(int cheapLevel)
    {
        throw new NotImplementedException();
    }
}
