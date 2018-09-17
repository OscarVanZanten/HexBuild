using Assets.Scripts.Level;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance { get; private set; }

    [Header("Optimization")]
    [Tooltip("The max radius of tiles being rendered(1 tile = 1 unit)")]
    public int RenderRadius;
    public float RenderRatio;
    private float RenderDistance { get { return (RenderRadius * RenderRatio) * size * (float)Math.Sqrt(2); } }
    [Tooltip("Amount of graphical level updates per second")]
    public float UpdateRate;
    [Tooltip("Amount of graphical level updates update cycle")]
    public int UpdateAmount;
    private int CurrentX, CurrentY, CurrentZ;
    private float PlotUpdateDelta;
    private bool PlotsCleared = false;

    [Header("Level Generation")]
    public GameObject SeaPlane;
    public GameObject plotPrefab;
    public Transform terrain;
    public int radius;
    public float size;
    public float heightAmplifier;
    public float heightOffset;
    public float baseTemperature;
    public float temperatureDegration;
    public float scale;
    public float TreeLine;
    public float SeaLevel;
    public float BeachLevel;
    public float BeachSize;
    public int MinAmountTrees;
    public int MaxAmountTrees;
    public Dictionary<HexLocation, Plot> plots;
    private int Diameter { get { return radius * 2 + 1; } }

    [Header("Buildings")]
    public GameObject FisherHouse;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        CurrentX = -RenderRadius;
        CurrentZ = -RenderRadius;
        CurrentY = -RenderRadius;

        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlotsCleared)
        {
            ClearPlots();
            PlotsCleared = true;
        }
        UpdatePlots();

    }

    private void UpdatePlots()
    {
        PlotUpdateDelta += Time.deltaTime;
        while (PlotUpdateDelta > 1 / UpdateRate)
        {
            PlotUpdateDelta -= 1 / UpdateRate;

            for (int i = 0; i < UpdateAmount;)
            {
                if (CurrentX < RenderRadius)
                {
                    CurrentX++;
                }

                if (CurrentX == RenderRadius)
                {
                    CurrentY++;
                    CurrentX = -RenderRadius;
                }
                if (CurrentY == RenderRadius)
                {
                    CurrentZ++;
                    CurrentY = -RenderRadius;
                }

                if (CurrentZ == RenderRadius)
                {
                    CurrentX = -RenderRadius;
                    CurrentZ = -RenderRadius;
                    CurrentY = -RenderRadius;
                }

                if (CurrentX + CurrentY + CurrentZ == 0)
                {
                    Plot camplot = GetCurrentCameraPlot();
                    if (camplot == null) return;

                    Plot plot = GetPlot(camplot.Location.X + CurrentX, camplot.Location.Y + CurrentY, camplot.Location.Z + CurrentZ);

                    if (plot != null)
                    {
                        i++;
                        UpdatePlot(plot);
                    }
                }
            }
        }
    }

    private void ClearPlots()
    {
        List<Plot> cleareingplots = plots.Values.ToList();

        if (cleareingplots != null)
        {
            foreach (Plot p in cleareingplots) p.ToggleHex(false);
        }
    }

    private void UpdateDrawnPlots()
    {
        List<Plot> plots = GetPlotsSurroundingCamera(RenderRadius);

        if (plots != null)
        {
            foreach (Plot p in plots) UpdatePlot(p);
        }
    }

    private void UpdatePlot(Plot p)
    {
        Vector2 cam = new Vector2(Camera.main.transform.position.x - p.transform.position.x, Camera.main.transform.position.z - p.transform.position.z);
        float dist = cam.magnitude;
        p.ToggleHex(dist <= RenderDistance);
    }

    private void LookAtPlot(Plot plot)
    {
        Vector3 plotLoc = new Vector3(plot.transform.position.x, Camera.main.transform.position.y, plot.transform.position.z);
        Camera.main.transform.position = plotLoc;
        ClearPlots();
    }

    private void Generate()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        ///General terrain
        GenerateSea();
        GeneratePlots();
        GenerateHills();
        GenerateTemperature();
        GenerateLakes();

        //Resources
        GenerateTrees();

        //Generate Buildings
        //  GenerateFisher();
    }

    private void GenerateSea()
    {
        float seascale = ((Diameter * size * Mathf.Sqrt(2)) / 10) + 10;
        SeaPlane.transform.localScale = new Vector3(seascale, 1, seascale);
        SeaPlane.transform.Translate(new Vector3(0, SeaLevel + 0.8f, 0));
    }

    private void GeneratePlots()
    {
        plots = new Dictionary<HexLocation, Plot>();

        for (int x = 0; x < Diameter; x++)
        {
            for (int y = 0; y < Diameter; y++)
            {
                for (int z = 0; z < Diameter; z++)
                {
                    int xx = x - radius;
                    int yy = y - radius;
                    int zz = z - radius;
                    int xyz = xx + yy + zz;

                    if (xyz == 0)
                    {
                        GameObject obj = GameObject.Instantiate(plotPrefab);
                        obj.transform.parent = terrain;
                        obj.transform.localPosition = new Vector3(xx * size, yy * size, zz * size);

                        Plot plot = obj.GetComponent<Plot>();
                        plot.Location = new HexLocation(xx, yy, zz);
                        plot.Type = PlotType.Grass;

                        plots.Add(plot.Location, plot);
                    }
                }
            }
        }
    }

    private void GenerateHills()
    {
        float[] map = NoiseMapGenerator.GeneratePerlinNoice(Diameter, Diameter, scale);

        foreach (Plot plot in plots.Values)
        {
            int pos = (plot.Location.X + radius) + (plot.Location.Y + radius) * Diameter;
            float height = Mathf.Pow((map[pos] + 1), heightAmplifier) + heightOffset;

            plot.Height = height;

            float max = Mathf.Abs(radius -
                    Mathf.Max(
                        Mathf.Abs(plot.Location.X),
                        Mathf.Abs(plot.Location.Y),
                        Mathf.Abs(plot.Location.Z)
                        ));


            if (BeachSize  - max >  0)
            {
                plot.Height = plot.Height / BeachSize * max;
            }
        }
    }

    private void GenerateTemperature()
    {
        float[] map = NoiseMapGenerator.GeneratePerlinNoice(Diameter, Diameter, scale);

        foreach (Plot plot in plots.Values)
        {
            int pos = (plot.Location.X + radius) + (plot.Location.Y + radius) * Diameter;
            float temp = (map[pos] + 1) * baseTemperature - temperatureDegration * plot.Height;

            plot.Temperature = temp;
        }
    }

    private void GenerateLakes()
    {
        foreach (Plot plot in plots.Values)
        {
            if (plot.Height <= SeaLevel)
            {
                plot.Type = PlotType.Water;
                // plot.Height = SeaLevel;
            }

        }

        foreach (Plot plot in plots.Values)
        {
            bool nearWater = GetSurroundingPlots(plot.Location).Where(x => x.Type == PlotType.Water).Count() > 0;

            if (plot.Height <= BeachLevel && plot.Height > SeaLevel && nearWater)
            {
                plot.Type = PlotType.Sand;
            }
        }
    }

    //private void GenerateBeaches()
    //{
    //    foreach (Plot plot in plots.Values)
    //    {
    //        if (plot.Location.X == radius || plot.Location.X == -radius ||
    //            plot.Location.Z == radius || plot.Location.Z == -radius ||
    //            plot.Location.Y == radius || plot.Location.Y == -radius)
    //        {
    //            plot.Type = PlotType.Water;
    //          //  plot.Height = SeaLevel;
    //        }
    //    }
    //}

    private void GenerateTrees()
    {
        foreach (Plot plot in plots.Values)
        {
            if (plot.Height <= TreeLine && plot.Type == PlotType.Grass)
            {
                int amount = NoiseMapGenerator.Random.Next(MinAmountTrees, MaxAmountTrees);

                plot.SetTrees(amount);
            }
        }
    }

    private void GenerateFisher()
    {
        foreach (Plot plot in plots.Values)
        {
            if (plot.Type != PlotType.Grass) continue;

            List<Plot> surrounding = GetSurroundingPlots(plot.Location);

            foreach (Plot surr in surrounding)
            {
                if (surr.Type == PlotType.Water)
                {
                    GameObject obj = GameObject.Instantiate(FisherHouse);
                    plot.SetBuilding(obj, 0);
                    break;
                }
            }
        }
    }

    public Plot GetPlot(int x, int y, int z)
    {
        HexLocation location = new HexLocation(x, y, z);
        Plot p = null;
        plots.TryGetValue(location, out p);
        return p;
    }

    public Plot GetPlot(HexLocation pos)
    {
        Plot p = null;
        plots.TryGetValue(pos, out p);
        return p;
    }

    public List<Plot> GetSurroundingPlots(HexLocation loc)
    {
        List<Plot> found = GetSurroundingPlots(1, loc);
        List<Plot> ordered = new List<Plot>();
        if (found.Count > 5) { ordered.Add(found[5]); }
        if (found.Count > 4) { ordered.Add(found[4]); }
        if (found.Count > 2) { ordered.Add(found[2]); }
        if (found.Count > 0) { ordered.Add(found[0]); }
        if (found.Count > 1) { ordered.Add(found[1]); }
        if (found.Count > 3) { ordered.Add(found[3]); }
        return ordered;
    }

   

    public List<Plot> GetSurroundingPlots(int x, int y, int z)
    {
        return GetSurroundingPlots(new HexLocation(x, y, z));
    }

    private List<Plot> GetSurroundingPlots(int radius, HexLocation loc)
    {
        List<Plot> p = new List<Plot>();

        for (int xx = -radius; xx <= radius; xx++)
        {
            for (int yy = -radius; yy <= radius; yy++)
            {
                for (int zz = -radius; zz <= radius; zz++)
                {
                    if (xx == 0 && zz == 0 && yy == 0)
                    {
                        continue;
                    }

                    Plot plot = GetPlot(xx + loc.X, yy + loc.Y, zz + loc.Z);
                    if (plot != null)
                    {
                        p.Add(plot);
                    }
                }
            }
        }
        return p;
    }

    private List<Plot> GetPlotsRing(int radius, int innerradius, HexLocation loc)
    {
        List<Plot> p = new List<Plot>();

        for (int xx = -RenderRadius; xx < RenderRadius; xx++)
        {
            for (int yy = -RenderRadius; yy < RenderRadius; yy++)
            {
                for (int zz = -RenderRadius; zz < RenderRadius; zz++)
                {
                    if (Mathf.Max(Math.Abs(xx), Math.Abs(yy), Math.Abs(zz)) < innerradius) continue;

                    Plot plot = GetPlot(xx + loc.X, yy + loc.Y, zz + loc.Z);
                    if (plot != null)
                    {
                        p.Add(plot);
                    }
                }
            }
        }
        return p;
    }

    private Plot GetCurrentCameraPlot()
    {
        RaycastHit hitUp;
        if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hitUp))
        {
            Plot plot = hitUp.transform.gameObject.GetComponentInParent<Plot>();

            return plot;
        }
        return null;
    }

    private List<Plot> GetPlotRingCamera(int radius, int innerRadius)
    {
        Plot plot = GetCurrentCameraPlot();

        if (plot != null)
        {
            return GetPlotsRing(radius, innerRadius, plot.Location);
        }
        return null;
    }

    private List<Plot> GetPlotsSurroundingCamera(int radius)
    {
        Plot plot = GetCurrentCameraPlot();

        if (plot != null)
        {
            return GetSurroundingPlots(radius, plot.Location);
        }
        return null;
    }
}
