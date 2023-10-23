using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCache
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public PlayerCache(Vector3 position, Quaternion rotation)
    {
        this.playerPosition = position;
        this.playerRotation = rotation;
    }
}

public struct EnemyTaskCache
{
    public ENEMY_TYPE type;
    public Vector3 position;
    public Quaternion rotation;
    public bool isComplete;

    public EnemyTaskCache(ENEMY_TYPE type, Vector3 position, Quaternion rotation, bool isComplete)
    {
        this.type = type;
        this.position = position;
        this.rotation = rotation;
        this.isComplete = isComplete;
        return;
    }
}

public struct AnimalTaskCache
{
    public ANIMAL_TYPE type;
    public Vector3 position;
    public Quaternion rotation;
    public bool isComplete;

    public AnimalTaskCache(ANIMAL_TYPE type, Vector3 position, Quaternion rotation, bool isComplete)
    {
        this.type = type;
        this.position = position;
        this.rotation = rotation;
        this.isComplete = isComplete;
        return;
    }
}

public static class Core
{
    // levelState
    private static LEVEL_STATE currentLevelState = LEVEL_STATE.Stopped;
    public static LEVEL_STATE GetLevelState() { return currentLevelState; }
    public static void SetLevelState(LEVEL_STATE newState) { currentLevelState = newState; }

    private static float playerFuel = 0f;
    public static float GetPlayerFuel() { return playerFuel; }
    public static void SetPlayerFuel(float newValue) { playerFuel = newValue; UpdateDisplays(); }
    public static void IncrementPlayerFuel(float increment) { playerFuel += increment; if (playerFuel < 0f) playerFuel = 0f; UpdateDisplays(); }

    private static float playerHealth = 0f;
    public static float GetPlayerHealth() { return playerHealth; }
    public static void SetPlayerHealth(float newValue) { playerHealth = newValue; UpdateDisplays(); }
    public static void IncrementPlayerHealth(float increment) { playerHealth += increment; if (playerHealth < 0f) playerHealth = 0f; UpdateDisplays(); }

    private static float playerGold = 0f;
    public static float GetPlayerGold() { return playerGold; }
    public static void SetPlayerGold(float newValue) { playerGold = newValue; UpdateDisplays(); }
    public static void IncrementPlayerGold(float increment) { playerGold += increment; if (playerGold < 0f) playerGold = 0f; UpdateDisplays(); }

    private static PlayerCache playerCache = new PlayerCache(Vector3.zero, Quaternion.identity);
    public static PlayerCache GetPlayerCache() { return playerCache; }
    public static void SetPlayerCache(PlayerCache newPlayerCache) { playerCache = newPlayerCache; }

    public static Vector3 GetPlayerPosition() { return playerCache.playerPosition; } // old, delete later
    public static void SetPlayerPosition(Vector3 position) { playerCache.playerPosition = position; } // old, delete later
    public static Quaternion GetPlayerRotation() { return playerCache.playerRotation; } // old, delete later
    public static void SetPlayerRotation(Quaternion rotation) { playerCache.playerRotation = rotation; } // old, delete later

    private static float radarRange = 7f; // placeholder
    public static float GetRadarRange() { return radarRange; }

    private static List<EnemyTaskCache> enemyTaskCacheList = new List<EnemyTaskCache>();
    public static void ClearEnemyTaskCache() { enemyTaskCacheList.Clear(); }
    public static void CacheEnemyTask(ENEMY_TYPE type, Vector3 position, Quaternion rotation, bool isComplete) { enemyTaskCacheList.Add(new EnemyTaskCache(type, position, rotation, isComplete)); }
    public static List<EnemyTaskCache> GetEnemyTaskCacheList() { return enemyTaskCacheList; }

    private static List<AnimalTaskCache> animalTaskCacheList = new List<AnimalTaskCache>();
    public static void ClearAnimalTaskCache() { animalTaskCacheList.Clear(); }
    public static void CacheAnimalTask(ANIMAL_TYPE type, Vector3 position, Quaternion rotation, bool isComplete) { animalTaskCacheList.Add(new AnimalTaskCache(type, position, rotation, isComplete)); }
    public static List<AnimalTaskCache> GetAnimalTaskCacheList() { return animalTaskCacheList; }

    // flags

    private static bool playerLoadStaged = false;
    public static bool IsPlayerLoadStaged() { return playerLoadStaged; }
    public static void StagePlayerLoad(bool newValue) { playerLoadStaged = newValue; /* bugfix pro playtest */ Core.SetIsOnRescue(false); }
    
    private static bool isOnRescue = false;
    public static bool GetIsOnRescue() { return isOnRescue; }
    public static void SetIsOnRescue(bool newValue) { isOnRescue = newValue; }

    // gameState
    private static string lastActiveSceneName = null;
    public static string GetLastActiveScene() { return lastActiveSceneName; }
    public static void SetLastActiveScene(string sceneName) { lastActiveSceneName = sceneName; }

    public static void Reset(LEVEL_STATE levelStateOverride = LEVEL_STATE.Stopped)
    {
        playerFuel = 100f;
        playerHealth = 100f;
        playerGold = 0f;
        animalCount = 3;
        enemyCount = 3;
        playerCache = new PlayerCache(Vector3.zero, Quaternion.identity);
        currentLevelState = levelStateOverride;
    }

    // util
    
    public static bool UpdateDisplays()
    {
        LevelDisplayController levelDisplayController = GameObject.Find("Canvas").GetComponent<LevelDisplayController>();
        if (levelDisplayController == null) return false;
        levelDisplayController.UpdateFuelDisplay();
        levelDisplayController.UpdateHealthDisplay();
        levelDisplayController.UpdateGoldDisplay();
        levelDisplayController.UpdateTaskDisplay();
        return true;
    }

    // playtest

    private static int animalCount = 3;
    public static int GetAnimalCount() {  return animalCount; }
    public static void SetAnimalCount(int newValue) { animalCount = newValue; }
    public static void IncrementAnimalCount(int increment) { animalCount += increment; if (animalCount < 0f) animalCount = 0; UpdateDisplays(); }

    private static int enemyCount = 3;
    public static int GetEnemyCount() { return enemyCount; }
    public static void SetEnemyCount(int newValue) { enemyCount = newValue; }
    public static void IncrementEnemyCount(int increment) { enemyCount += increment; if (enemyCount < 0f) enemyCount = 0; UpdateDisplays(); }
}
