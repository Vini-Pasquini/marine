using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] Button[] levelButtons;

    private void Start()
    {
        for (int index = Mathf.Min(Core.GetLevelProgress(), levelButtons.Length - 1); index >= 0; index--)
        {
            levelButtons[index].interactable = true;
        }
    }
}
