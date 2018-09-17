using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Structure : MonoBehaviour
{
    public StructureType Type;

    public GameObject Prefab;
    public GameObject PossiblePrefab;
    public GameObject FailedPrefab;

    private GameObject Shown;
}
