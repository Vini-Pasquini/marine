using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private ENEMY_TYPE enemyType;
    public ENEMY_TYPE GetEnemyType() { return enemyType; }
    public void SetEnemyType(ENEMY_TYPE newType) { enemyType = newType; }

    private bool isTaskComplete = false;
    public bool IsTaskComplete() { return isTaskComplete; }
    public void SetTaskComplete(bool newValue) { isTaskComplete = newValue; }

    private void Start()
    {
        /* enemy init */
        // select enemy type ()
        // load enemy model
        // apply convex mesh collider
        /* enemy highlight init */
        // dup model mesh as enemy highlight
        // set enemy highlight to "enemy" layer
        // change material to SelectionHighlightMat
        // meshrenderer.enabled = false
    }

    private void Update()
    {
        
    }
}
