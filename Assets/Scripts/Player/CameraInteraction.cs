using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteraction : MonoBehaviour
{
    public Camera Camera;
    public BuildingSelector Selector;

    private PlotGrid Current;
    public bool IsBuilding
    {
        get
        {
            return Current != null ? Current.Status == BuildStatus.Preview : false;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Current != null)
        {
            if (Current.Status == BuildStatus.None)
            {
                PlaceInitialPreview();
            }

            if (Current.Status == BuildStatus.Preview)
            {
                Vector3 pointed = GetPointedLocation();
                Vector3 pointedLoc = new Vector3(pointed.x, 0, pointed.z);
                Vector3 currentLoc = new Vector3(Current.transform.position.x, 0, Current.transform.position.z);

                Vector3 dir = pointedLoc - currentLoc;
                Quaternion rotation = Quaternion.LookRotation(dir);
                int i = (int)((rotation.eulerAngles.y) / Current.RotationPerBuilding);

                if (pointed != Vector3.zero)
                {
                    UpdatePreview(i);
                }

                if (Input.GetMouseButtonDown(0)) //check if the LMB is clicked
                {
                    Place(i);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelPreview();
                }
            }
        }
        else
        {
            Current = GetClickedGrid();
        }
    }

    private void PlaceInitialPreview()
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                Current.PlacePreviewRoad();
                break;
            case StructureType.Square:
                Current.PlacePreviewSquare();
                break;
            default:
                Current.PlacePreviewBuilding(Selector.Selected);
                break;
        }
    }

    private void UpdatePreview(int i)
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                Current.PlacePreviewRoadPosition(i);
                break;
            case StructureType.Square:
                break;
            default:
                Current.PlacePreviewBuildingPosition(i);
                break;
        }
    }

    private void Place(int i)
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                Current.RemovePreviewRoad();
                Current.PlaceRoad(i);
                break;
            case StructureType.Square:
                Current.RemovePreviewSquare();
                Current.PlaceSquare();
                break;
            default:
                Current.RemovePreviewBuilding();
                Current.PlaceBuilding(Selector.Selected, i);
                break;
        }
        Current = null;
    }

    private void CancelPreview()
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                Current.RemovePreviewRoad();
                break;
            case StructureType.Square:
                Current.RemovePreviewSquare();
                break;
            default:
                Current.RemovePreviewBuilding();
                break;
        }
        Current = null;
    }

    /// <summary>
    /// Gets the current selected plot
    /// </summary>
    /// <returns></returns>
    private PlotGrid GetClickedGrid()
    {
        if (Input.GetMouseButtonDown(0)) //check if the LMB is clicked
        {
            RaycastHit hit;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Plot p = hit.transform.gameObject.GetComponentInParent<Plot>();
                return p.GetComponentInChildren<PlotGrid>();
            }
        }
        return null;
    }

    private Vector3 GetPointedLocation()
    {
        RaycastHit hit;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
