using UnityEngine;

public class StructureFactory : MonoBehaviour {
    public static StructureFactory Instance { get; private set; }

    public GameObject Road;
    public GameObject Square;
    public GameObject Building;

	// Use this for initialization
	void Start () {
        Instance = this;
	}

    public Building GetBuilding(StructureType type)
    {
        switch (type)
        {
            case StructureType.Building:
                return GameObject.Instantiate(Building).GetComponent<Building>();
            
              
                
        }

        return null;
    }

    public Road GetRoad()
    {
        return GameObject.Instantiate(Road).GetComponent<Road>();
    }

    public Square GetSquare()
    {
        return GameObject.Instantiate(Square).GetComponent<Square>();
    }
}
