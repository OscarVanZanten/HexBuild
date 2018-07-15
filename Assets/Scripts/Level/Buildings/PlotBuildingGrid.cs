using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildStatus { None, Preview }

public class PlotBuildingGrid : MonoBehaviour
{

    private Building[] buildings = new Building[6];
    private Building previewBuilding;

    public BuildStatus Status { get; set; }

    public int RotationPerBuilding { get { return 360 / buildings.Length; } }

    public void PlaceBuilding(BuildingType building, int i)
    {
        if (CanPlaceBuilding(i))
        {
            Building b = BuildingFactory.Instance.GetBuilding(building);
            buildings[i] = b;
            buildings[i].transform.parent = transform;
            buildings[i].transform.localPosition = new Vector3();
            buildings[i].transform.rotation = Quaternion.Euler(Vector3.up * RotationPerBuilding * (i - 1));
        } 
    }

    public bool CanPlaceBuilding(int i)
    {
        if (i < 0 || i >= buildings.Length)
        {
            return false;
        }
        return buildings[i] == null;
    }

    public void PlacePreviewBuilding(BuildingType building, int i)
    {
        PlacePreviewBuilding(building);
        PlacePreviewBuildingPosition(i);
    }

    public void PlacePreviewBuilding(BuildingType building)
    {
        Building b = BuildingFactory.Instance.GetBuilding(building);
        this.Status = BuildStatus.Preview;
        this.previewBuilding = b;
        this.previewBuilding.transform.parent = transform;
        this.previewBuilding.transform.localPosition = new Vector3();
    }

    public void PlacePreviewBuildingPosition(int pos)
    {
        previewBuilding.transform.rotation = Quaternion.Euler(Vector3.up * RotationPerBuilding * (pos - 1));
    }

    public void RemovePreviewBuilding()
    {
        Status = BuildStatus.None;
        if (previewBuilding != null)
        {
            Destroy(previewBuilding.gameObject);
        }
    }

}
