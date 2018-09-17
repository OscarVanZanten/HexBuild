using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Road : Building
{
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
}

