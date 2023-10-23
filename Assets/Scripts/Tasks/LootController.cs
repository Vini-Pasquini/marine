using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    private LOOT_TYPE lootType;
    public LOOT_TYPE GetLootType() { return lootType; }
    public void SetLootType(LOOT_TYPE newType) { lootType = newType; }

    private bool isTaskComplete = false;
    public bool IsTaskComplete() { return isTaskComplete; }
    public void SetTaskComplete(bool newValue) { isTaskComplete = newValue; }

    private void Start()
    {
        /* loot init */
        // select loot type ()
        // load loot model
        // apply convex mesh collider
        /* loot highlight init */
        // dup model mesh as loot highlight
        // set loot highlight to "loot" layer
        // change material to SelectionHighlightMat
        // meshrenderer.enabled = false
    }

    private void Update()
    {

    }
}
