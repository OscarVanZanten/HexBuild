using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingSelectButton : MonoBehaviour {

    public BuildingSelector Selector;
    public StructureType Type;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBuildingType()
    {
        Selector.Selected = Type;
    }
}
