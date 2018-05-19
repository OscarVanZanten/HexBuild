using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject plotPrefab;
    [SerializeField] private Transform terrain;
    [SerializeField] private int radius;
    [SerializeField] private int size;

    private int diameter { get { return radius * 2 + 1; } }

    private Plot[] plots;

    // Use this for initialization
    void Start()
    {
        plots = new Plot[diameter * diameter * diameter];



        for (int x = 0; x < diameter; x++)
        {
            for (int y = 0; y < diameter; y++)
            {
                for (int z = 0; z < diameter; z++)
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
                        obj.transform.localPosition = new Vector3(xx, yy, zz);
                    }
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 CubeToAxial(Vector3 cube)
    {
        var x = cube.x;
        var y = cube.z;
        return new Vector2(x, y);
    }

    public Vector3 AxialToCube(Vector2 hex)
    {
        var x = hex.x;
        var y = hex.y;
        var z = -x - y;
        return new Vector3(x, y, z);
    }
}
