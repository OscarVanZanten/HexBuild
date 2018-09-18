using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildStatus { None, Preview }

public class PlotGrid : MonoBehaviour
{
    public Plot Plot;

    public Building[] Buildings = new Building[6];
    private Building previewBuilding;

    public Square Square;
    private Square previewSquare;

    public Road[] Roads = new Road[6];
    private Road previewRoad;

    public BuildStatus Status { get; set; }

    public int RotationPerBuilding { get { return 360 / Buildings.Length; } }

    public void PlaceSquare()
    {
        if (CanPlaceSquare())
        {
            Square b = StructureFactory.Instance.GetSquare();
            Square = b;
            Square.PlotGrid = this;
            Square.transform.parent = transform;
            Square.transform.localPosition = new Vector3();
        }
    }

    public bool CanPlaceSquare()
    {
        return Square == null;
    }

    public void PlacePreviewSquare()
    {
        Square b = StructureFactory.Instance.GetSquare();
        this.Status = BuildStatus.Preview;
        this.previewSquare = b;
        this.previewSquare.transform.parent = transform;
        this.previewSquare.transform.localPosition = new Vector3();
    }

    public void RemovePreviewSquare()
    {
        Status = BuildStatus.None;
        if (previewSquare != null)
        {
            Destroy(previewSquare.gameObject);
        }
    }

    public void PlaceRoad(int i)
    {
        if (CanPlaceRoad(i))
        {
            Road b = StructureFactory.Instance.GetRoad();
            Roads[i] = b;
            Roads[i].PlotGrid = this;
            Roads[i].transform.parent = transform;
            Roads[i].transform.localPosition = new Vector3();
            Roads[i].transform.rotation = Quaternion.Euler(Vector3.up * RotationPerBuilding * (float)(i - 1.5));

            Roads[i].Center = Square;

            var SurroundingPlots = Grid.Instance.GetSurroundingPlots(Plot.Location);

            int ii = i == 0 ? Roads.Length - 1 : i - 1;
            {
                var leftPlot = SurroundingPlots[ii];
                var grid = leftPlot.PlotGrid;

                int iii = i + 2;
                int plotIndex = (iii >= Roads.Length) ? iii - Roads.Length : iii;

                var newRoad = grid.Roads[plotIndex];

                if (newRoad != null)
                {
                    Roads[i].Left = newRoad;
                    newRoad.Right = Roads[i];
                }

                //if (Roads[i].Left != null)
                //{
                //    Roads[i].Left.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                //};
            }


            {
                var rightPlot = SurroundingPlots[i];
                var grid = rightPlot.PlotGrid;

                int iii = i + 4;
                int plotIndex = (iii >= Roads.Length) ? iii - Roads.Length : iii;

                if (grid.Roads[plotIndex] != null)
                {
                    Roads[i].Right = grid.Roads[plotIndex];
                    grid.Roads[plotIndex].Left = Roads[i];
                }
                //if (Roads[i].Right != null)
                //{
                //    Roads[i].Right.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                //};
            }
        }
    }

    public bool CanPlaceRoad(int i)
    {
        if (i < 0 || i >= Roads.Length || Square == null)
        {
            return false;
        }
        return Roads[i] == null;
    }

    public void PlacePreviewRoad(int i)
    {
        PlacePreviewRoad();
        PlacePreviewRoadPosition(i);
    }

    public void PlacePreviewRoad()
    {
        Road b = StructureFactory.Instance.GetRoad();
        this.Status = BuildStatus.Preview;
        this.previewRoad = b;
        this.previewRoad.transform.parent = transform;
        this.previewRoad.transform.localPosition = new Vector3();
    }

    public void PlacePreviewRoadPosition(int i)
    {
        previewRoad.transform.rotation = Quaternion.Euler(Vector3.up * RotationPerBuilding * (float)(i - 1.5));
    }

    public void RemovePreviewRoad()
    {
        Status = BuildStatus.None;
        if (previewRoad != null)
        {
            Destroy(previewRoad.gameObject);
        }
    }

    public void PlaceBuilding(StructureType building, int i)
    {
        if (CanPlaceBuilding(i))
        {
            Building b = StructureFactory.Instance.GetBuilding(building);
            Buildings[i].PlotGrid = this;
            Buildings[i] = b;
            Buildings[i].transform.parent = transform;
            Buildings[i].transform.localPosition = new Vector3();
            Buildings[i].transform.rotation = Quaternion.Euler(Vector3.up * RotationPerBuilding * (i - 1));

            Buildings[i].Left = Buildings[i - 1 < 0 ? Buildings.Length - 1 : i - 1];
            Buildings[i].Right = Buildings[i + 1 > Buildings.Length - 1 ? 0 : i + 1];
        }
    }

    public bool CanPlaceBuilding(int i)
    {
        if (i < 0 || i >= Buildings.Length)
        {
            return false;
        }
        return Buildings[i] == null;
    }

    public void PlacePreviewBuilding(StructureType building, int i)
    {
        PlacePreviewBuilding(building);
        PlacePreviewBuildingPosition(i);
    }

    public void PlacePreviewBuilding(StructureType building)
    {
        Building b = StructureFactory.Instance.GetBuilding(building);
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
