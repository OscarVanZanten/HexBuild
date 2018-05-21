using Assets.Scripts.Level;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
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

    private List<Plot> plots;

    // Use this for initialization
    void Start()
    {
        float seed = Mathf.Min((float)NoiseMapGenerator.Random.NextDouble() +0.25f, 1);
        Generate(seed);
    }

    // Update is called once per frame
    void Update()
    {

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
        plots = new List<Plot>();

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
                        obj.transform.localPosition = new Vector3(xx * size, yy  * size, zz  * size);

                        Plot plot = obj.GetComponent<Plot>();
                        plot.Location = new HexLocation(xx, zz);

                        plots.Add(plot);
                    }
                }
            }
        }
    }

    private void GenerateHills(float seed)
    {
        float[] map = NoiseMapGenerator.GeneratePerlinNoice(Diameter, Diameter, (float)seed, scale);

        foreach (Plot plot in plots)
        {
            int pos = (plot.Location.Q + radius) + (plot.Location.R + radius) * Diameter;
            float height = Mathf.Pow((map[pos] + 1), heightAmplifier) + heightOffset;

            plot.Height = height;
        }
    }

    private void GenerateLakes()
    {
        foreach (Plot plot in plots)
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
        foreach (Plot plot in plots)
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
        foreach (Plot plot in plots)
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
        return plots.Where(p => p.Location.Q == q && p.Location.R == r).FirstOrDefault();
    }

    public Plot GetPlot(HexLocation pos)
    {
        return GetPlot((int)pos.Q, (int)pos.R);
    }

    public List<Plot> GetSurroundingPlots(HexLocation loc)
    {
        return GetSurroundingPlots(loc.Q, loc.R);
    }

    public List<Plot> GetSurroundingPlots(int q, int r)
    {
        List<Plot> plots = new List<Plot>();

        Plot p1 = GetPlot(q, r - 1);
        Plot p2 = GetPlot(q - 1, r);
        Plot p3 = GetPlot(q - 1, r + 1);
        Plot p4 = GetPlot(q, r + 1);
        Plot p5 = GetPlot(q + 1, r);
        Plot p6 = GetPlot(q + 1, r - 1);
        if (p1 != null) plots.Add(p1);
        if (p2 != null) plots.Add(p2);
        if (p3 != null) plots.Add(p3);
        if (p4 != null) plots.Add(p4);
        if (p5 != null) plots.Add(p5);
        if (p6 != null) plots.Add(p6);

        return plots;
    }
}
