using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour {

    public BuildingType Selected { get; private set; }

	// Use this for initialization
	void Start () {
        Selected = BuildingType.Building;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
