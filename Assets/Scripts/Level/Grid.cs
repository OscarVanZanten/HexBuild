﻿using Assets.Scripts.Level;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject plotPrefab;
    [SerializeField] private Transform terrain;
    [SerializeField] private int radius;
    [SerializeField] private float size;
    [SerializeField] private float heightAmplifier;
    [SerializeField] private float heightOffset;
    [SerializeField] private float scale;
    [SerializeField] private float TreeLine;
    [SerializeField] private int MinAmountTrees;
    [SerializeField] private int MaxAmountTrees;

    private int Diameter { get { return radius * 2 + 1; } }

    private List<Plot> plots;

    // Use this for initialization
    void Start()
    {
        float seed = (float)NoiseMapGenerator.Random.NextDouble();
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
        GeneratePlots();
        GenerateHills(seed);
        GenerateTrees();
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
                        GameObject obj = Instantiate(plotPrefab);
                        obj.transform.parent = terrain;
                        obj.transform.localPosition = new Vector3(xx * size, yy * size, zz * size);

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

    private void GenerateTrees()
    {
        foreach (Plot plot in plots)
        {
            if (plot.Height <= TreeLine)
            {
                int amount = NoiseMapGenerator.Random.Next(MinAmountTrees, MaxAmountTrees);
                plot.SetTrees(amount);
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



}
