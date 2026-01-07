using WhiteLab.PCConfigurator.Requirenments;
using WhiteLab.PCConfigurator.Steps;

namespace WhiteLab.PCConfigurator.Core;

public class PCConfigurator
{
    public async Task<PCConfigResult> Configure(Requirements requirements, CancellationToken ct)
    {
        var container = await PCGlobalContainer.GetContainerAsync(ct);
        var relationships = await PCGlobalContainer.GetRelationShipsAsync(ct);

        List<IStep> steps = [
            new CPUSelectionStep(requirements, container, relationships)
        ];
        if (!requirements.ExcludeGpu) steps.Insert(0, new GPUSelectionStep(requirements, container, relationships));
        foreach (var step in steps) step?.SetupBaseComponents();
        if (requirements.ExcludeGpu)  container.Gpus = [];

        bool Filtering(int stepIndex = 0)
        {
            if (steps.Count == stepIndex) return true;

            var step = steps[stepIndex];
            for (int i = 0; i <= step.MaxCheapLevel; i++)
            {
                var r = step.FilterAndCheap(i);
                if (!r) continue;
                r &= Filtering(stepIndex + 1);
                if (r) return true;
            }
            step.ClearContainer();
            return false;
        }
        
        var resultCnf = Filtering();
        if (!container.IsComposed(requirements.ExcludeGpu)) return PCConfigResult.Set(null, "По заданым требования не удалось подобрать комплектующие");
        var result = resultCnf ? CreatePCConfig(container) : null;
        return PCConfigResult.Set(result, "Бюджета не хватило сборку пк удовлетворяющего только системным требованиям");
    }

    private static PCConfig CreatePCConfig(PCContainer container)
    {
        var pc = new PCConfig();

        pc.Components =
        [
            new PCConfig.PCComponent(container.Cpus[0], null, container.CpusInfo),

        ];
        if (container.Gpus.Any()) pc.Components.Insert(0, new PCConfig.PCComponent(container.Gpus[0], string.Join(',', container.Gpus.Take(3).SelectMany(m => m.Models.Select(mm => $"{m.Seria} {mm}"))), container.GpusInfo));

        return pc;
    }
}
