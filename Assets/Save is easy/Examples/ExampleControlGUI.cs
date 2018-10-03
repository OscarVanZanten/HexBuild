using SaveIsEasy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleControlGUI : MonoBehaviour {
    //to avoid setting an empty name
    private string sceneFileName;

    private void Start() {
        sceneFileName = SaveIsEasyAPI.SceneFileName;
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Save Game")) {
            SaveIsEasyAPI.SaveAll();
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "Load Game")) {
            SaveIsEasyAPI.LoadAll();
        }

        if (GUI.Button(new Rect(10, 90, 150, 30), "Menu")) {
            SceneManager.LoadScene(0);
        }

        //to avoid setting an empty name
        if (string.IsNullOrEmpty(sceneFileName) == false) {
            sceneFileName = SaveIsEasyAPI.SceneFileName;
        }

        sceneFileName = GUI.TextField(new Rect(180, 10, 150, 30), sceneFileName);

        if(string.IsNullOrEmpty(sceneFileName) == false) {
            SaveIsEasyAPI.SceneFileName = sceneFileName;
        }

    }
}