using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{

    [SerializeField] private Material[] SolidMaterials;
    [SerializeField] private Material[] FadeMaterials;

    private MeshRenderer renderer;
    private bool rendered;
    private float currentFade;
    public bool IsSolid { get; internal set; }

    // Use this for initialization
    void Start()
    {
        this.renderer = GetComponent<MeshRenderer>();
        this.rendered = true;
        this.currentFade = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFade(float fade)
    {
        if (!this.gameObject.activeSelf) return;
        if (fade.Equals(currentFade)) return;

        currentFade = fade;

        if (fade == 0)
        {
            if (rendered)
            {
                if (!renderer.material.Equals(SolidMaterials)) renderer.materials = SolidMaterials;
                renderer.enabled = false;
                rendered = false;
            }
            return;
        }


        if (!rendered)
        {
            rendered = true;
            renderer.enabled = true;
        }

        if (fade == 1)
        {
            if (!renderer.material.Equals(SolidMaterials)) renderer.materials = SolidMaterials;
            IsSolid = true;
            return;
        }
        IsSolid = false;
        if (!renderer.material.Equals(FadeMaterials)) renderer.materials = FadeMaterials;

        foreach (Material material in renderer.materials)
        {
            Color color = new Color(material.color.r, material.color.g, material.color.b, fade);
            material.color = color;
        }
    }
}
