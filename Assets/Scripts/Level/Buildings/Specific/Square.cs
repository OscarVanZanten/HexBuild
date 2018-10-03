using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Square : Structure
{
    private Road[] roads = new Road[6];

    public void ConnectRoad(Road road, int i)
    {
        roads[i] = road;

    }

    public void DisconnectRoad( int i)
    {
        roads[i] = null;
    }
}

