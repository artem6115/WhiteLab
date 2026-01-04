using WhiteLab.PCConfigurator.Requirenments;
using WhiteLab.PCConfigurator.Steps;

namespace WhiteLab.PCConfigurator.Core;

public class PCConfigurator
{
    public async Task<PCConfigResult> Configure(Requirements requirements, CancellationToken ct)
    {
        var container = await PCGlobalContainer.GetContainerAsync(ct);
        var relationships = await PCGlobalContainer.GetRelationShipsAsync(ct);

        IStep[] steps = [new GPUSelectionStep(requirements, container, relationships)];
        foreach (var step in steps) step.SetupBaseComponents();

        bool Filtering(int stepIndex = 0)
        {
            if (steps.Length == stepIndex) return true;

            var step = steps[stepIndex];
            for (int i = 0; i <= step.MaxCheapLevel; i++)
            {
                var r = step.FilterAndCheap(i);
                if (!r) continue;
                r &= Filtering(stepIndex + 1);
                if (r) return true;
            }

            return false;
        }
        if (!container.IsComposed()) return PCConfigResult.Set(null, "По заданым требования не удалось подобрать комплектующие");
        var resultCnf = Filtering();
        var result = resultCnf ? CreatePCConfig(container) : null;
        return PCConfigResult.Set(result, "Бюджета не хватило сборку пк удовлетворяющего только системным требованиям");
    }

    private static PCConfig CreatePCConfig(PCContainer container)
    {
        var pc = new PCConfig();

        pc.Components =
        [
            new PCConfig.PCComponent("Видеокарта", $"{container.Gpus[0].Seria} {container.Gpus[0].Models[0]} {container.Gpus[0].RAM}GB", container.Gpus[0].Price,string.Join(',', container.Gpus.Take(3).SelectMany(m => m.Models.Select(mm => $"{m.Seria} {mm}"))), container.GpusInfo)
        ];

        return pc;
    }
}
