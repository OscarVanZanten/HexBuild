﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Quit()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
