using Assets.Scripts.Level;
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

    private int Diameter { get { return radius * 2 + 1; } }

    private List<Plot> plots;

    // Use this for initialization
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Generate()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        GeneratePlots();
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

    public Plot GetPlot(int q, int r)
    {
        return plots.Where(p => p.Location.Q == q && p.Location.R == r).FirstOrDefault();
    }

    public Plot GetPlot(HexLocation pos)
    {
        return GetPlot((int)pos.Q, (int)pos.R);
    }


  
}
