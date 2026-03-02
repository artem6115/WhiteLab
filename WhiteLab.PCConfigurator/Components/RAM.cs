using System.Text.Json.Serialization;

namespace WhiteLab.PCConfigurator.Components;

public class RAM : IComponent
{
    public string Type => "Оперативная память";
    public string Name {  get; set; }
    public int Price {  get; set; }
    public string RAMType { get; set; }
    public int Capacity { get; set; }
    public int Frequency { get; set; }
    public bool Xmp { get; set; }
    public bool Expo { get; set; }
    public string Timings { get; set; }
    public string Color { get; set; }
    public bool Rgb { get; set; }

    [JsonIgnore]
    public int ModulsCount { get; set; }

    public override string ToString()
    {
        var rgb = Rgb ? "имеет" : "не имеет";
        var prof = (Expo && Xmp) ? "Имеет expo и xmp профили" : Expo ? "Имеет expo профиль" : Xmp ? "Имеет xmp профиль" : "";
        return $"Оперативная память в количестве: {ModulsCount}\nПамять типа {RAMType}, с суммарным объемом памяти {Capacity} ГБ, частотой {Frequency} и таймингами {Timings}\nЦвет: {Color}, Rgb: {rgb}\n{prof}";
    }

}
