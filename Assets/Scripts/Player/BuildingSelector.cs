using Assets.Scripts.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    public CameraInteraction cameraInteraction;
    public StructureType Selected { get; set; }

    // Use this for initialization
    void Start()
    {
        Selected = StructureType.Road;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameDataStorage.Instance.Save(Grid.Instance.GetGridData());
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            var instance = GameDataStorage.Instance;
        }

        if (cameraInteraction.IsBuilding)
        {
            return;
        }
    }
}
