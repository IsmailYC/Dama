using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour {
    public string scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }

    public void DelaySceneLoad()
    {
        Invoke("LoadScene", 2.0f);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
