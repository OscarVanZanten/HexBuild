using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraInteraction : MonoBehaviour
{
    public Camera Camera;
    public BuildingSelector Selector;

    private PlotGrid CurrentClickedPlot;
    private PlotGrid CurrentHoveredPlot;

    public bool IsBuilding { get; set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (CurrentClickedPlot != null)
        {
            if (CurrentHoveredPlot != null)
            {
                CurrentHoveredPlot.Plot.Selected = false;
            }

            if (CurrentClickedPlot.Status == BuildStatus.None)
            {
                PlaceInitialPreview();
            }

            if (CurrentClickedPlot.Status == BuildStatus.Preview)
            {
                Vector3 pointed = GetPointedLocation();
                Vector3 pointedLoc = new Vector3(pointed.x, 0, pointed.z);
                Vector3 currentLoc = new Vector3(CurrentClickedPlot.transform.position.x, 0, CurrentClickedPlot.transform.position.z);

                Vector3 dir = pointedLoc - currentLoc;
                Quaternion rotation = Quaternion.LookRotation(dir);
                int i = (int)((rotation.eulerAngles.y + 30) / CurrentClickedPlot.RotationPerBuilding);

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
            if (IsBuilding)
            {
                CurrentClickedPlot = GetClickedGrid();

                if (CurrentHoveredPlot != null)
                {
                    CurrentHoveredPlot.Plot.Selected = false;
                }

                CurrentHoveredPlot = GetHoveredOverPlotGrid();
                if (CurrentHoveredPlot != null)
                {
                    CurrentHoveredPlot.Plot.Selected = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsBuilding = false;
                CurrentHoveredPlot.Plot.Selected = false;
            }
        }
    }

    private void PlaceInitialPreview()
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                CurrentClickedPlot.PlacePreviewRoad();
                break;
            case StructureType.Square:
                CurrentClickedPlot.PlacePreviewSquare();
                break;
            default:
                CurrentClickedPlot.PlacePreviewBuilding(Selector.Selected);
                break;
        }
    }

    private void UpdatePreview(int i)
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                CurrentClickedPlot.PlacePreviewRoadPosition(i);
                break;
            case StructureType.Square:
                break;
            default:
                CurrentClickedPlot.PlacePreviewBuildingPosition(i);
                break;
        }
    }

    private void Place(int i)
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                CurrentClickedPlot.RemovePreviewRoad();
                CurrentClickedPlot.PlaceRoad(i);
                break;
            case StructureType.Square:
                CurrentClickedPlot.RemovePreviewSquare();
                CurrentClickedPlot.PlaceSquare();
                break;
            default:
                CurrentClickedPlot.RemovePreviewBuilding();
                CurrentClickedPlot.PlaceBuilding(Selector.Selected, i);
                break;
        }
        CurrentClickedPlot = null;
    }

    private void CancelPreview()
    {
        switch (Selector.Selected)
        {
            case StructureType.Road:
                CurrentClickedPlot.RemovePreviewRoad();
                break;
            case StructureType.Square:
                CurrentClickedPlot.RemovePreviewSquare();
                break;
            default:
                CurrentClickedPlot.RemovePreviewBuilding();
                break;
        }
        CurrentClickedPlot = null;
    }

    /// <summary>
    /// Gets the current selected plot
    /// </summary>
    /// <returns></returns>
    private PlotGrid GetClickedGrid()
    {
        if (Input.GetMouseButtonDown(0)) //check if the LMB is clicked
        {
            return GetHoveredOverPlotGrid();
        }
        return null;
    }

    private PlotGrid GetHoveredOverPlotGrid()
    {
        RaycastHit hit;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Plot p = hit.transform.gameObject.GetComponentInParent<Plot>();
                Debug.Log(hit.transform.gameObject.name);
                return p.GetComponentInChildren<PlotGrid>();
            }
            else
            {
                Debug.Log("Hit UI");
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
