using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GridData
{
    public int Radius { get; set; }
    public float HeightAmplifier { get; set; }
    public float BaseTemperature { get; set; }
    public float TemperatureDegration { get; set; }
    public float TreeLine { get; set; }
    public float SeaLevel { get; set; }
    public float BeachLevel { get; set; }
    public float BeachSize { get; set; }

    public List<PlotData> Plots { get; set; }
}
