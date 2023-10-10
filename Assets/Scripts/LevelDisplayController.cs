using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDisplayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fuelDisplay;

    public void UpdateFuelDisplay(float newValue = -1f)
    {
        if (fuelDisplay == null) return;
        fuelDisplay.text = "FUEL:\n" + (newValue >= 0f ? newValue : Core.GetPlayerFuel());
    }

    [SerializeField] private TextMeshProUGUI healthDisplay;

    public void UpdateHealthDisplay(float newValue = -1f)
    {
        if (healthDisplay == null) return;
        healthDisplay.text = "HP:\n" + (newValue >= 0f ? newValue : Core.GetPlayerHealth());
    }

    [SerializeField] private TextMeshProUGUI goldDisplay;

    public void UpdateGoldDisplay(float newValue = -1f)
    {
        if (goldDisplay == null) return;
        goldDisplay.text = "GOLD:\n" + (newValue >= 0f ? newValue : Core.GetPlayerGold());
    }

    [SerializeField] private TextMeshProUGUI taskDisplay;

    public void UpdateTaskDisplay(float newValue = -1f)
    {
        if (taskDisplay == null) return;
        taskDisplay.text = "TASKS:\n" + (newValue >= 0f ? newValue : Core.GetAnimalCount() + Core.GetEnemyCount());
    }
}
