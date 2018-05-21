using Assets.Scripts.Level;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour {

    [SerializeField] private Transform StoneLayer;
    [SerializeField] private Transform DirtLayer;
    [SerializeField] private float DirtPercentage;

    [SerializeField] private GameObject TreePrefab;
    [SerializeField] private float MinScaleTree;
    [SerializeField] private float MaxScaleTree;

    [SerializeField] private Transform ResourcesPosition;
    private List<GameObject> Resources = new List<GameObject>();

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
            this.transform.Translate(new Vector3(0, (value-currentHeight), 0), Space.Self);
            StoneLayer.localScale = new Vector3(1, value *(1-DirtPercentage), 1);
            StoneLayer.localPosition = new Vector3(0,  -value * DirtPercentage-1, 0);
            DirtLayer.localScale = new Vector3(1, value * DirtPercentage, 1);
        }
    }

    public void SetTrees(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject tree = GameObject.Instantiate(TreePrefab);
            float x = (float)NoiseMapGenerator.Random.NextDouble() * 5;
            float z = (float)NoiseMapGenerator.Random.NextDouble() * 5;
            float scale = MinScaleTree + (float)(NoiseMapGenerator.Random.NextDouble() * (MaxScaleTree - MinScaleTree));
            tree.transform.parent = ResourcesPosition;
            tree.transform.localPosition = new Vector3(x, 0, z);
            tree.transform.localScale = new Vector3(scale, scale, scale);
            tree.transform.localRotation = Quaternion.Euler(0, (float)(NoiseMapGenerator.Random.NextDouble() * 360.0), 0);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
