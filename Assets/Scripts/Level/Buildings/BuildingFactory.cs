using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : MonoBehaviour {
    public static BuildingFactory Instance { get; private set; }

    public GameObject Building;

	// Use this for initialization
	void Start () {
        Instance = this;
	}

    public Building GetBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Building:
                return GameObject.Instantiate(Building).GetComponent<Building>();
            case BuildingType.Road:
                break;
        }

        return null;
    }
}
