using System;
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

public struct FilthTaskCache
{
    public FILTH_TYPE type;
    public bool isComplete;

    public FilthTaskCache(FILTH_TYPE type, bool isComplete)
    {
        this.type = type;
        this.isComplete = isComplete;
        return;
    }
}

public struct LootTaskCache
{
    public LOOT_TYPE type;
    public bool isComplete;

    public LootTaskCache(LOOT_TYPE type, bool isComplete)
    {
        this.type = type;
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

    private static int currentBattleIndex = -1;
    public static int GetBattleIndex() { return currentBattleIndex; }
    public static void SetBattleIndex(int newIndex) { currentBattleIndex = newIndex; }
    public static bool SetCurrentBattleVictory()
    {
        if (currentBattleIndex < 0 || currentBattleIndex >= enemyTaskCacheList.Count) return false;
        EnemyTaskCache enemyTaskCache = enemyTaskCacheList[currentBattleIndex];
        enemyTaskCache.isComplete = true;
        enemyTaskCacheList[currentBattleIndex] = enemyTaskCache;
        return true;
    }

    // flags

    private static bool playerLoadStaged = false;
    public static bool IsPlayerLoadStaged() { return playerLoadStaged; }
    public static void StagePlayerLoad(bool newValue) { playerLoadStaged = newValue; /* bugfix pro playtest */ Core.SetIsOnRescue(false); }
    
    private static bool isOnRescue = false;
    public static bool GetIsOnRescue() { return isOnRescue; }
    public static void SetIsOnRescue(bool newValue) { isOnRescue = newValue; }

    // gameState
    private static SCENES lastActiveScene = SCENES.MainMenu;
    public static SCENES GetLastActiveScene() { return lastActiveScene; }
    public static void SetLastActiveScene(SCENES scene) { lastActiveScene = scene; }

    public static void Reset(LEVEL_STATE levelStateOverride = LEVEL_STATE.Stopped)
    {
        playerFuel = 100f;
        playerHealth = 100f;
        playerGold = 0f;
        animalCount = 1;
        enemyCount = 1;
        filthCount = 1;
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

    private static int animalCount = 1;
    public static int GetAnimalCount() {  return animalCount; }
    public static void SetAnimalCount(int newValue) { animalCount = newValue; UpdateDisplays(); }
    public static void IncrementAnimalCount(int increment) { animalCount += increment; if (animalCount < 0f) animalCount = 0; UpdateDisplays(); }

    private static int enemyCount = 1;
    public static int GetEnemyCount() { return enemyCount; }
    public static void SetEnemyCount(int newValue) { enemyCount = newValue; UpdateDisplays(); }
    public static void IncrementEnemyCount(int increment) { enemyCount += increment; if (enemyCount < 0f) enemyCount = 0; UpdateDisplays(); }

    private static int filthCount = 1;
    public static int GetFilthCount() { return filthCount; }
    public static void SetFilthCount(int newValue) { filthCount = newValue; UpdateDisplays(); }
    public static void IncrementFilthCount(int increment) { filthCount += increment; if (filthCount < 0f) filthCount = 0; UpdateDisplays(); }

    private static int levelProgress = 0;
    public static int GetLevelProgress() { Debug.Log($"get {levelProgress}"); return levelProgress; }
    public static void SetLevelProgress(int newValue) { Debug.Log($"before {levelProgress}"); if (newValue >= 0) levelProgress = newValue; Debug.Log($"before {levelProgress}"); }
    public static void IncrementLevelProgress(string levelName)
    {
        Debug.Log($"before {levelProgress}");
        int currentLevel = 0;
        Debug.Log($"current level {currentLevel}");
        int.TryParse(levelName.Split("_")[1], out currentLevel);
        Debug.Log($"try parse done, current level {currentLevel}");
        if (levelProgress != currentLevel - 1) return;
        Debug.Log($"level progress check");
        levelProgress++;
        Debug.Log($"after {levelProgress}");
    }
}
