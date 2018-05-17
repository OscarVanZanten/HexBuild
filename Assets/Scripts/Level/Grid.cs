using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject plotPrefab;
    [SerializeField] private Transform terrain;
    [SerializeField] private int diameter;
    [SerializeField] private int size;

    private Plot[] plots;

    // Use this for initialization
    void Start()
    {
        plots = new Plot[diameter * diameter * diameter];

        for (int x = -diameter/2; x < diameter/2; x++)
        {
            for (int y = -diameter/2; y < diameter/2; y++)
            {
                for (int z = -diameter / 2; z < diameter/2; z++)
                {
                    if ((x + y + z)+1 != 0) continue;

                    GameObject obj = Instantiate(plotPrefab);
                    obj.transform.parent = terrain;
                    obj.transform.position = new Vector3(x, y, z);

                   // plots[x + (y * radius) + (z * radius * radius)] = obj.GetComponent<Plot>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
