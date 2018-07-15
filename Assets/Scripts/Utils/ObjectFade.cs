using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{

    [SerializeField] private Material[] SolidMaterials;
    [SerializeField] private Material[] FadeMaterials;

    private static Dictionary<string, Material> CachedMaterials = new Dictionary<string, Material>();

    private new MeshRenderer renderer;
    private bool rendered;
    private float currentFade;


    // Use this for initialization
    void Start()
    {
        this.renderer = GetComponent<MeshRenderer>();
        this.rendered = true;
        this.currentFade = 1;
    }

    public void SetFade(float fade)
    {
        fade = (float)Math.Round(fade, 1);

        if (!this.gameObject.activeInHierarchy) return;
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

            return;
        }
        if (!renderer.material.Equals(FadeMaterials)) renderer.materials = FadeMaterials;

        for(int i =0; i < renderer.materials.Length;i++)
        {
            Material material = renderer.materials[i];
            string name = material.name + fade;

            if (CachedMaterials.ContainsKey(name))
            {
                renderer.materials[i] = CachedMaterials[name];
            }
            else
            {
                Color color = new Color(material.color.r, material.color.g, material.color.b, fade);
                material.color = color;
                CachedMaterials.Add(name, material);
            }

        }
    }
}
