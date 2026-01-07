using System.Text.Json.Serialization;
using WhiteLab.PCConfigurator.Steps;

namespace WhiteLab.PCConfigurator.Components;

public class CPU : IComponent
{
    public string Type => "Прочессор";
    public string Name => Model;

    public string Model { get; set; }
    public int Tir { get; set; }
    public string CoresAndThreads
    {
        set
        {
            _coresAndThreads = value;
            var p = value.Replace("P", "").Replace("T", "").Split('/');
            Cores = int.Parse(p[0]);
            Threads = int.Parse(p[1]);
        }
    }
    private string _coresAndThreads;
    public string Frequency { get; set; }
    public string BoostedFrequency { get; set; }
    public int WorkScore { get; set; }
    public int GameScore { get; set; }
    public string Socket { get; set; }
    public int Price { get; set; }

    [JsonIgnore]
    public int Cores { get; set; }

    [JsonIgnore]
    public int Threads { get; set; }

    public override string ToString()
    {
        var n = Environment.NewLine;
        var overcloking = CPUSelectionStep.OverclockingSupport(Model) ? "имеет" : "не имеет";
        return $"Процессор {_coresAndThreads} (ядра/потоки) с сокетом {Socket}{n}имеет частоту {Frequency}, {overcloking} поддержку разгона, рейтинг - {(WorkScore+ GameScore)/2}/100";
    }
}