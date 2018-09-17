using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour {
    public CameraInteraction cameraInteraction;
    public StructureType Selected { get; private set; }

	// Use this for initialization
	void Start () {
        Selected = StructureType.Road;
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraInteraction.IsBuilding)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Selected = StructureType.Road;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Selected = StructureType.Building;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Selected = StructureType.Square;
        }
    }
}
