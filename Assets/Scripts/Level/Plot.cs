﻿using Assets.Scripts.Level;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlotType { Ground, Water }

public class Plot : MonoBehaviour
{
    [Header("Optimization")]
    [SerializeField] private float RenderDistance;
    [SerializeField] private float FadeStartDistance;

    [Header("Ground Layers")]
    [SerializeField] private Transform StoneLayer;
    [SerializeField] private Transform DirtLayer;
    [SerializeField] private float DirtPercentage;

    [Header("Top Layers")]
    [SerializeField] private Transform BuildingPosition;
    [SerializeField] private Transform ResourcesPosition;
    [SerializeField] private GameObject GrassTop;
    [SerializeField] private GameObject WaterTop;

    [Header("Resources")]
    [SerializeField] private GameObject TreePrefab;
    [SerializeField] private float MinScaleTree;
    [SerializeField] private float MaxScaleTree;

    private List<GameObject> resources = new List<GameObject>();

    private ObjectFade[] FadeObject;
    private bool rendered;

    private PlotType type = PlotType.Ground;
    public PlotType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            switch (type)
            {
                case PlotType.Ground:
                    GrassTop.SetActive(true);
                    WaterTop.SetActive(false);
                    break;
                case PlotType.Water:
                    GrassTop.SetActive(false);
                    WaterTop.SetActive(true);
                    break;
            }
        }
    }

    public HexLocation Location { get; set; }
    private float height;
    public float Height
    {
        get
        {
            return height;
        }
        set
        {
            this.height = value;
            float currentHeight = this.transform.position.y;
            this.transform.Translate(new Vector3(0, (value - currentHeight), 0), Space.Self);
            StoneLayer.localScale = new Vector3(1, value * (1 - DirtPercentage), 1);
            StoneLayer.localPosition = new Vector3(0, -value * DirtPercentage - 1, 0);
            DirtLayer.localScale = new Vector3(1, value * DirtPercentage, 1);
        }
    }

    public void SetTrees(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject tree = GameObject.Instantiate(TreePrefab);
            float x = (float)NoiseMapGenerator.Random.NextDouble() * 4;
            float z = (float)NoiseMapGenerator.Random.NextDouble() * 4;
            float scale = MinScaleTree + (float)(NoiseMapGenerator.Random.NextDouble() * (MaxScaleTree - MinScaleTree));
            tree.transform.parent = ResourcesPosition;
            tree.transform.localPosition = new Vector3(x, 0, z);
            tree.transform.localScale = new Vector3(scale, scale, scale);
            tree.transform.localRotation = Quaternion.Euler(0, (float)(NoiseMapGenerator.Random.NextDouble() * 360.0), 0);
        }
    }

    public void SetBuilding(GameObject obj, int rotation)
    {
        obj.transform.parent = BuildingPosition;
        obj.transform.localPosition = new Vector3();
        obj.transform.localRotation = Quaternion.Euler(0, 30 * rotation, 0);
    }

    // Use this for initialization
    void Start()
    {
        FadeObject = GetComponentsInChildren<ObjectFade>();
        rendered = true;
    }

    // Update is called once per frame
    void Update()
    {
       
        //Vector2 camPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);
       // Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
        float dist = (Camera.main.transform.position - transform.position).magnitude;


        if (dist < FadeStartDistance)
        {
            FadeRenders(1);
            return;
        }

        if (dist >= RenderDistance )
        {
            rendered = false;
            FadeRenders(0);
        }

        if (dist > FadeStartDistance && dist < RenderDistance)
        {
            float ratio = 1 - (dist - FadeStartDistance) / (RenderDistance - FadeStartDistance);
            FadeRenders(ratio);
        }

     
    }

    private void FadeRenders(float fade)
    {
        foreach (ObjectFade objectFade in FadeObject)
        {
            if (fade == 1 && objectFade.IsSolid) continue;

            objectFade.SetFade(fade);
        }
    }
}
