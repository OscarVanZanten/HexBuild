using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Road : Building
{
    public GameObject Ramp;
    private Square center;
    public Square Center
    {
        get
        {
            return center;
        }
        set
        {
            center = value;
            UpdatePlacement();
           
        }


    }

    public override void UpdatePlacement()
    {
        float leftH = Left != null ? Left.PlotGrid.Plot.Height : 0;
        float rightH = Right != null ? Right.PlotGrid.Plot.Height : 0;

        float height = Mathf.Max(leftH, rightH, PlotGrid.Plot.Height) - PlotGrid.Plot.Height;

        Debug.Log("-----------------");
        Debug.Log(PlotGrid.Plot.Height);
        Debug.Log(Left);
        Debug.Log(Right);
        //Debug.Log(height);
       // Debug.Log(leftH);
       /// Debug.Log(rightH);
       // Debug.Log(PlotGrid.Plot.Height);
        if (height != 0)
        {
            Ramp.transform.localScale = new Vector3(Ramp.transform.localScale.x, Ramp.transform.localScale.y, height * 0.5f);
        }
    }
}

