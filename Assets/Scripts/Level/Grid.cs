using Assets.Scripts.Level;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Optimization")]
    [Tooltip("The max radius of tiles being rendered(1 tile = 1 unit)")]
    [SerializeField] private int RenderRadius;
    private float RenderDistance { get { return (RenderRadius - 5) * size * (float)Math.Sqrt(2); } }
    [SerializeField] private float FadeDistanceStart;
    [Tooltip("Amount of graphical level updates per second")]
    [SerializeField] private float UpdateRate;
    [SerializeField] private int UpdateAmount;
    private List<Plot> PlotUpdateList;
    private float PlotUpdateDelta;
    private int CurrentPlotIndex;
    private bool PlotsCleared = false;

    [Header("Level Generation")]
    [SerializeField] private GameObject plotPrefab;
    [SerializeField] private Transform terrain;
    [SerializeField] private int radius;
    [SerializeField] private float size;
    [SerializeField] private float heightAmplifier;
    [SerializeField] private float heightOffset;
    [SerializeField] private float scale;
    [SerializeField] private float TreeLine;
    [SerializeField] private float SeaLevel;
    [SerializeField] private int MinAmountTrees;
    [SerializeField] private int MaxAmountTrees;
    private Dictionary<HexLocation, Plot> plots;
    private int Diameter { get { return radius * 2 + 1; } }

    [Header("Buildings")]
    [SerializeField] private GameObject FisherHouse;

    // Use this for initialization
    void Start()
    {
        float seed = Mathf.Min((float)NoiseMapGenerator.Random.NextDouble() + 0.33f, 1);
        Generate(seed);
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
            //UpdateDrawnPlots();
            if (PlotUpdateList == null)
            {
                PlotUpdateList = GetPlotsSurroundingCamera(RenderRadius);
                return;
            }

            for (int i = 0; i < UpdateAmount; i++)
            {
                CurrentPlotIndex++;
                if (CurrentPlotIndex != PlotUpdateList.Count)
                {
                    UpdatePlot(PlotUpdateList[CurrentPlotIndex]);
                }
                else
                {
                    PlotUpdateList = GetPlotsSurroundingCamera(RenderRadius);
                    CurrentPlotIndex = 0;
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

        p.ToggleHex(dist <= RenderDistance );
        //  p.UpdateFade(Mathf.Min(dist / FadeStartDistance, 1));
    }

    private void Generate(float seed)
    {
        GenerateTerrain(seed);
    }

    private void GenerateTerrain(float seed)
    {
        ///General terrain
        GeneratePlots();
        GenerateHills(seed);
        GenerateLakes();

        //Resources
        GenerateTrees();

        //Generate Buildings
        //  GenerateFisher();
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

                        plots.Add(plot.Location, plot);
                    }
                }
            }
        }
    }

    private void GenerateHills(float seed)
    {
        float[] map = NoiseMapGenerator.GeneratePerlinNoice(Diameter, Diameter, (float)seed, scale);

        foreach (Plot plot in plots.Values)
        {
            int pos = (plot.Location.X + radius) + (plot.Location.Y + radius) * Diameter;
            float height = Mathf.Pow((map[pos] + 1), heightAmplifier) + heightOffset;

            plot.Height = height;
        }
    }

    private void GenerateLakes()
    {
        foreach (Plot plot in plots.Values)
        {
            if (plot.Height <= SeaLevel)
            {
                plot.Type = PlotType.Water;

                List<Plot> surrounding = GetSurroundingPlots(plot.Location);
                Plot higher = surrounding.Where(p => p.Type == PlotType.Water).OrderBy(p => p.Height).LastOrDefault();

                if (higher != null)
                {
                    foreach (Plot p in surrounding.Where(pl => pl.Type == PlotType.Water))
                    {
                        p.Height = higher.Height;
                    }
                    plot.Height = higher.Height;
                }
            }
            else
            {
                plot.Type = PlotType.Ground;
            }
        }
    }

    private void GenerateTrees()
    {
        foreach (Plot plot in plots.Values)
        {
            if (plot.Height <= TreeLine && plot.Type == PlotType.Ground)
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
            if (plot.Type != PlotType.Ground) continue;

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
        return GetSurroundingPlots(1, loc);
    }

    public List<Plot> GetSurroundingPlots(int x, int y, int z)
    {
        return GetSurroundingPlots(new HexLocation(x, y, z));
    }

    private List<Plot> GetSurroundingPlots(int radius, HexLocation loc)
    {
        List<Plot> p = new List<Plot>();

        for (int xx = -RenderRadius; xx < RenderRadius; xx++)
        {
            for (int yy = -RenderRadius; yy < RenderRadius; yy++)
            {
                for (int zz = -RenderRadius; zz < RenderRadius; zz++)
                {
                    Plot plot = GetPlot(xx + loc.X, yy + loc.Y, zz + loc.Z);
                    if (plot != null)
                    {
                        p.Add(plot);
                        // if (plot.Location == loc) continue;
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

    private List<Plot> GetPlotRingCamera(int radius, int innerRadius)
    {
        RaycastHit hitUp;
        if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hitUp))
        {
            Plot plot = hitUp.transform.gameObject.GetComponentInParent<Plot>();

            if (plot != null)
            {
                return GetPlotsRing(RenderRadius, innerRadius, plot.Location);
            }
        }
        return null;
    }

    private List<Plot> GetPlotsSurroundingCamera(int radius)
    {
        RaycastHit hitUp;
        if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hitUp))
        {
            Plot plot = hitUp.transform.gameObject.GetComponentInParent<Plot>();

            if (plot != null)
            {
                return GetSurroundingPlots(RenderRadius, plot.Location);
            }
        }
        return null;
    }
}
