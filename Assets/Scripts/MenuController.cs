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
        if (SceneManager.GetActiveScene().name == SCENES.MainMenu.ToString()) // game initialization check
        {
            //Screen.SetResolution(1600, 900, false);
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
        if (playerTransform == null) return false;
        Core.SetPlayerCache(new PlayerCache(playerTransform.position, playerTransform.rotation));

        GameObject.FindObjectOfType<LevelManager>().CacheTasks();

        return true;
    }

    public void OnChangeSceneButtonPress(string scene)
    {
        SCENES newScene = (SCENES)Enum.Parse(typeof(SCENES), scene);
        if (currentScene.name == SCENES.LevelMap.ToString())
        {
            Core.SetLevelState(LEVEL_STATE.Stopped);
            // new level check
            if (newScene.ToString().StartsWith("Level_")) Core.Reset(levelStateOverride: LEVEL_STATE.Ongoing);
        }
        if (newScene == SCENES.BattleScene) this.CacheLevelInfo();
        if (newScene.ToString().StartsWith("Level_") && currentScene.name == SCENES.BattleScene.ToString()) Core.StagePlayerLoad(true); // back to level check
        if (newScene == SCENES.LevelMap) { Core.ClearAnimalTaskCache(); Core.ClearEnemyTaskCache(); } // ph
        Core.SetLastActiveScene((SCENES)Enum.Parse(typeof(SCENES), currentScene.name));
        SceneManager.LoadScene(newScene.ToString());
    }

    public void OnBackButtonPress()
    {
        SCENES lastScene = Core.GetLastActiveScene();
        Core.SetLastActiveScene((SCENES)Enum.Parse(typeof(SCENES), currentScene.name));

        if (lastScene.ToString().StartsWith("Level_") && currentScene.name == SCENES.BattleScene.ToString()) Core.StagePlayerLoad(true); // back to level check
        if (lastScene == SCENES.LevelMap) { Core.SetLevelState(LEVEL_STATE.Stopped); Core.ClearAnimalTaskCache(); Core.ClearEnemyTaskCache(); } // ph cache

        SceneManager.LoadScene(lastScene.ToString());
    }

    public void OnRescueButtonPress()
    {
        // will do more stuff here in the future
        bool rescueFlag = Core.GetIsOnRescue();
        RescueInteraction(rescueFlag);
    }

    private bool RescueInteraction(bool isOnRescue)
    {
        CameraController cameraController = Camera.main.GetComponentInParent<CameraController>();
        if (cameraController == null) return false;

        GameObject cachedHoveredObject = cameraController.GetCachedHoveredObject();
        if (cachedHoveredObject == null) return false;

        AnimalController animalController = cachedHoveredObject.GetComponentInParent<AnimalController>();
        if (animalController == null) return false;

        GameObject playerBoat = GameObject.Find("PlayerBoat");
        if (playerBoat == null) return false;

        if(animalController.GetIsSafe()) return false;

        animalController.SetTargetObject(isOnRescue ? null : playerBoat);
        Core.SetIsOnRescue(!isOnRescue);
        return true;
    }

    public void OnFuelButtonPress()
    {
        Core.SetPlayerFuel(100);
    }

    public void OnMarkButtonPress()
    {
        MarkInteraction();
    }

    private bool MarkInteraction()
    {
        CameraController cameraController = Camera.main.GetComponentInParent<CameraController>();
        if (cameraController == null) return false;

        GameObject cachedHoveredObject = cameraController.GetCachedHoveredObject();
        if (cachedHoveredObject == null) return false;

        switch (cachedHoveredObject.layer)
        {
            case (int)LAYERS.Enemy:
                cameraController.SetEnemyLocatorTarget(cameraController.GetEnemyLocatorTarget() == cachedHoveredObject ? null : cachedHoveredObject);
                return true;
            case (int)LAYERS.Animal:
                cameraController.SetAnimalLocatorTarget(cameraController.GetAnimalLocatorTarget() == cachedHoveredObject ? null : cachedHoveredObject);
                return true;
            case (int)LAYERS.Fuel:
                cameraController.SetFuelLocatorTarget(cameraController.GetFuelLocatorTarget() == cachedHoveredObject ? null : cachedHoveredObject);
                return true;
            default: return false;
        }
    }
}
