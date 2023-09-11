using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Screen.SetResolution(1600, 900, false);
        }
    }

    public void OnChangeSceneButtonPress(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
}
