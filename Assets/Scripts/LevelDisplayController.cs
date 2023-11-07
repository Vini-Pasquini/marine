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
    [SerializeField] private TextMeshProUGUI animalTaskDisplay;
    [SerializeField] private TextMeshProUGUI enemyTaskDisplay;
    [SerializeField] private TextMeshProUGUI filthTaskDisplay;

    public void UpdateTaskDisplay(float newValue = -1f)
    {
        int animalCount = Core.GetAnimalCount();
        int enemyCount = Core.GetEnemyCount();
        int filthCount = 0;

        if (taskDisplay != null) taskDisplay.text = "TASKS:\n" + (newValue >= 0f ? newValue : animalCount + enemyCount);
        if (animalTaskDisplay != null) animalTaskDisplay.text = animalCount <= 0 ? "- <color=#00FF00>Todos os animais SALVOS</color>" : $"- Salve {animalCount} Animais (Leve-os para a area segura)";
        if (enemyTaskDisplay != null) enemyTaskDisplay.text = enemyCount <= 0? "- <color=#00FF00>Todos os cacadores DERROTADOS</color>" : $"- Derrote {enemyCount} Barcos de cacadores";
        if (filthTaskDisplay != null) filthTaskDisplay.text = filthCount <= 0 ? "- ? ? ?" : "- ? ? ?";
    }
}
