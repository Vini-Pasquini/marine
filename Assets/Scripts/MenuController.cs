using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Scene currentScene;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") // game initialization check
        {
            Screen.SetResolution(1600, 900, false);
            Core.Reset();
        }
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private bool CacheLevelInfo()
    {
        Core.SetPlayerTransform(GameObject.Find("PlayerBoat").transform);
        return true;
    }

    public void OnChangeSceneButtonPress(string newScene)
    {
        if (currentScene.name == "LevelMap" || newScene.StartsWith("Level_")) Core.Reset(); // new level check
        if (newScene == "BattleScene") this.CacheLevelInfo();
        Core.SetLastActiveScene(currentScene.name);
        SceneManager.LoadScene(newScene);
    }

    private bool LoadCachedLevel() // placeholder test
    {
        GameObject playerBoat = GameObject.Find("PLayerBoat");
        if (playerBoat != null) return false;
        Transform cachedTransform = Core.GetPlayerTransform();
        playerBoat.transform.position = cachedTransform.position;
        playerBoat.transform.rotation = cachedTransform.rotation;
        return true;
    }

    public void OnBackButtonPress()
    {
        string nextScene = Core.GetLastActiveScene();
        Core.SetLastActiveScene(currentScene.name);

        if (nextScene.StartsWith("Level_") && currentScene.name == "BattleScene") Core.SetPlayerTransformLoadStaged(true);

        SceneManager.LoadScene(nextScene);
    }
}
