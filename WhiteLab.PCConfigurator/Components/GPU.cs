using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace WhiteLab.PCConfigurator.Components;

public class GPU : IComponent
{
    public string Type => "Видеокарта";
    public string Name => $"{Seria} {RAM}GB {Models[0]}";
    public bool InStock { get; set; }
    public string Seria { get; set; }
    public string Tir { get; set; }
    public int RAM { get; set; }
    public int FansCount { get; set; }
    public string Color { get; set; }
    public int Price { get; set; }
    public int Frequency { get; set; }
    public int Power { get; set; }
    public int Width { get; set; }
    public bool Led { get; set; }

    [JsonIgnore]
    public List<string> Models { get; set; }

    [JsonPropertyName("Models")]
    public string ModelsString { set { Models = value.Split(',').Select(m => m.Trim()).ToList(); } }

    public override string ToString()
    {
        var n = Environment.NewLine;
        var led = Led ? "имеет" : "не имеет";
        var rgb = Models[0].ToLower().Contains("rgb") ? "имеет" : "не имеет";

        return $"Видеокарта с {FansCount} кулерами, {RAM}GB, цвет - {Color}, {led} подсветку и {rgb} RGB{n}TDP - {Power}, Width - {Width}";
    }

}
