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
    [SerializeField] private int RenderRadius;
    [SerializeField] private float RenderDistance;
    [SerializeField] private float FadeStartDistance;
    private bool cleared = false;

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

    [Header("Buildings")]
    [SerializeField] private GameObject FisherHouse;

    private int Diameter { get { return radius * 2 + 1; } }

    private Dictionary<HexLocation, Plot> plots;

    // Use this for initialization
    void Start()
    {
        float seed = Mathf.Min((float)NoiseMapGenerator.Random.NextDouble() + 0.25f, 1);
        Generate(seed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!cleared)
        {
            ClearPlots();
            cleared = true;
        }
        UpdateDrawnPlots();
    }

    private void ClearPlots()
    {
        RaycastHit hitUp;
        if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hitUp))
        {
            Plot plot = hitUp.transform.gameObject.GetComponentInParent<Plot>();

            if (plot == null)
            {
                throw new UnityException("Not above a tile to clear the level");
            }

            List<Plot> renderedPlots = GetSurroundingPlots(RenderRadius-1, plot.Location);

            foreach (Plot p in plots.Values)
            {
                if (!renderedPlots.Contains(p) && !p.Equals(plot))
                {
                    p.UpdateFade(0);
                    p.ToggleHex(false);
                }
            }
        }
    }

    private void UpdateDrawnPlots()
    {
        RaycastHit hitUp;
        if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out hitUp))
        {
            Plot plot = hitUp.transform.gameObject.GetComponentInParent<Plot>();

            if (plot != null)
            {
                List<Plot> pl = GetSurroundingPlots(RenderRadius, plot.Location);

                foreach (Plot p in pl)
                {
                    float dist = (Camera.main.transform.position - p.transform.position).magnitude;

                    if (dist >= RenderDistance)
                    {
                        p.UpdateFade(0);
                        p.ToggleHex(false);
                    }
                    else if (dist > FadeStartDistance && dist < RenderDistance)
                    {
                        p.UpdateFade(1 - (dist - FadeStartDistance) / (RenderDistance - FadeStartDistance));
                        p.ToggleHex(true);
                    }
                    else
                    {
                        p.UpdateFade(1);
                    }
                }
            }
        }
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
                        float xxx = xx * ((xx % 2 == 0) ? 1 : 1.5f);
                        float zzz = zz * ((zz % 2 == 0) ? 1 : 1.5f);
                        GameObject obj = GameObject.Instantiate(plotPrefab);
                        obj.transform.parent = terrain;
                        obj.transform.localPosition = new Vector3(xx * size, yy * size, zz * size);

                        Plot plot = obj.GetComponent<Plot>();
                        plot.Location = new HexLocation(xx, zz);
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
            int pos = (plot.Location.Q + radius) + (plot.Location.R + radius) * Diameter;
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

    public Plot GetPlot(int q, int r)
    {
        HexLocation location = new HexLocation(q, r);
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

    public List<Plot> GetSurroundingPlots(int q, int r)
    {
        return GetSurroundingPlots(new HexLocation(q, r));
    }

    private List<Plot> GetSurroundingPlots(int radius, HexLocation loc)
    {
        List<Plot> p = new List<Plot>();

        for (int qq = -RenderRadius; qq <= RenderRadius; qq++)
        {
            for (int rr = -RenderRadius; rr <= RenderRadius; rr++)
            {
                Plot plot = GetPlot(qq + loc.Q, rr + loc.R);
                if (plot == null) continue;
                if (plot.Location == loc) continue;

                p.Add(plot);
            }
        }
        return p;
    }
}
