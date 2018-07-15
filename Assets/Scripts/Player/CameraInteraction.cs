using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteraction : MonoBehaviour
{
    public Camera Camera;
    public BuildingSelector Selector; 

    private PlotBuildingGrid Current;

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
                Current.PlacePreviewBuilding(Selector.Selected);
            }
            
            if(Current.Status == BuildStatus.Preview)
            {
                Vector3 pointed = GetPointedLocation();
                Vector3 pointedLoc = new Vector3(pointed.x, 0, pointed.z);
                Vector3 currentLoc = new Vector3(Current.transform.position.x, 0, Current.transform.position.z);

                Vector3 dir = pointedLoc - currentLoc;
                Quaternion rotation = Quaternion.LookRotation(dir);
                int i = (int)((rotation.eulerAngles.y) / Current.RotationPerBuilding) ;

                if (pointed != Vector3.zero)
                {
                    Current.PlacePreviewBuildingPosition(i);
                }

                if (Input.GetMouseButtonDown(0)) //check if the LMB is clicked
                {
                    Current.RemovePreviewBuilding();
                    Current.PlaceBuilding(Selector.Selected, i);
                    Current = null;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Current.RemovePreviewBuilding();
                    Current = null;
                }
            }
        }
        else
        {
            Current = GetClickedGrid();
        }
    }

    /// <summary>
    /// Gets the current selected plot
    /// </summary>
    /// <returns></returns>
    private PlotBuildingGrid GetClickedGrid()
    {
        if (Input.GetMouseButtonDown(0)) //check if the LMB is clicked
        {
            RaycastHit hit;
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Plot p = hit.transform.gameObject.GetComponentInParent<Plot>();
                return p.GetComponentInChildren<PlotBuildingGrid>();
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
