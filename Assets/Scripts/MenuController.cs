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
        Transform playerTransform = GameObject.Find("PlayerBoat").transform;
        Core.SetPlayerPosition(playerTransform.position);
        Core.SetPlayerRotation(playerTransform.rotation);
        return true;
    }

    public void OnChangeSceneButtonPress(string newScene)
    {
        if (currentScene.name == "LevelMap")
        {
            Core.SetLevelState(LEVEL_STATE.Stopped);
            // new level check
            if (newScene.StartsWith("Level_")) Core.Reset(levelStateOverride: LEVEL_STATE.Ongoing);
        }
        if (newScene == "BattleScene") this.CacheLevelInfo();
        Core.SetLastActiveScene(currentScene.name);
        SceneManager.LoadScene(newScene);
    }

    public void OnBackButtonPress()
    {
        string nextScene = Core.GetLastActiveScene();
        Core.SetLastActiveScene(currentScene.name);

        if (nextScene.StartsWith("Level_") && currentScene.name == "BattleScene") Core.StagePlayerLoad(true); // back to level check
        if (nextScene.StartsWith("LevelMap")) Core.SetLevelState(LEVEL_STATE.Stopped);

        SceneManager.LoadScene(nextScene);
    }

    public void OnRescueButtonPress()
    {
        // will do more stuff here in the future
        if (RescueInteraction()) Debug.Log("Now Rescuing Animal");
    }

    private bool RescueInteraction()
    {

        CameraController cameraController = Camera.main.GetComponentInParent<CameraController>();
        if (cameraController == null) return false;

        GameObject cachedHoveredObject = cameraController.GetCachedHoveredObject();
        if (cachedHoveredObject == null) return false;

        AnimalController animalController = cachedHoveredObject.GetComponentInParent<AnimalController>();
        if (animalController == null) return false;

        GameObject playerBoat = GameObject.Find("PlayerBoat");
        if (playerBoat == null) return false;

        animalController.SetTargetObject(playerBoat);
        return true;
    }
}
