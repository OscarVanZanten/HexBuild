using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Structure
{
    private Building left;
    private Building right;

    public Building Left
    {
        get
        {
            return left;
        }
        set
        {
            left = value;
            UpdatePlacement();
            
        }
    }
    public Building Right
    {
        get
        {
            return right;
        }
        set
        {
            right = value;
            UpdatePlacement();
          
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void UpdatePlacement()
    {

    }
}
