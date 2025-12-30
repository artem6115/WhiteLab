namespace WhiteLab.PCConfigurator.Components;

public class GPU
{
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
    public bool led { get; set; }
    public List<string> Models { get; set; }
}
