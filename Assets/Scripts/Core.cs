using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    private static float playerFuel = 0f;
    public static float GetPlayerFuel() { return playerFuel; }
    public static void SetPlayerFuel(float newValue) { playerFuel = newValue; }

    private static float playerHealth = 0f;
    public static float GetPlayerHealth() { return playerHealth; }
    public static void SetPlayerHealth(float newValue) { playerHealth = newValue; }

    private static float playerGold = 0f;
    public static float GetPlayerGold() { return playerGold; }
    public static void SetPlayerGold(float newValue) { playerGold = newValue; }

    public static void Reset()
    {
        playerFuel = 0f;
        playerHealth = 0f;
        playerGold = 0f;
    }
}
