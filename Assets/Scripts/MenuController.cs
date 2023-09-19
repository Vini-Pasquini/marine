using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Awake()
    {
        AwakeMenu();
    }

    private bool AwakeMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu") return false;
        Screen.SetResolution(1600, 900, false);
        Core.Reset();
        return true;
    }

    // need to check if is comming from LevelMap or BattleScene... skip if BattleScene
    private bool StartNewLevel(string levelScene)
    {
        if (SceneManager.GetActiveScene().name != "LevelMap" || levelScene != "TestLevel") return false; // placeholder
        Core.Reset();
        return true;
    }

    public void OnChangeSceneButtonPress(string newScene)
    {
        StartNewLevel(newScene);
        SceneManager.LoadScene(newScene);
    }
}
