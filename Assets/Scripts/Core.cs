using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    private static float playerFuel = 0f;
    public static float GetPlayerFuel() { return playerFuel; }
    public static void SetPlayerFuel(float newValue) { playerFuel = newValue; }
    public static void IncrementPlayerFuel(float increment) { playerFuel += increment; }

    private static float playerHealth = 0f;
    public static float GetPlayerHealth() { return playerHealth; }
    public static void SetPlayerHealth(float newValue) { playerHealth = newValue; }
    public static void IncrementPlayerHealth(float increment) { playerHealth += increment; }

    private static float playerGold = 0f;
    public static float GetPlayerGold() { return playerGold; }
    public static void SetPlayerGold(float newValue) { playerGold = newValue; }
    public static void IncrementPlayerGold(float increment) { playerGold += increment; }

    public static void Reset()
    {
        playerFuel = 100f;
        playerHealth = 100f;
        playerGold = 0f;
    }
}
