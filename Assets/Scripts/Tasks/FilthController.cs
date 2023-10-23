using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilthController : MonoBehaviour
{
    private FILTH_TYPE filthType;
    public FILTH_TYPE GetFilthType() { return filthType; }
    public void SetFilthType(FILTH_TYPE newType) { filthType = newType; }

    private bool isTaskComplete = false;
    public bool IsTaskComplete() { return isTaskComplete; }
    public void SetTaskComplete(bool newValue) { isTaskComplete = newValue; }

    private void Start()
    {
        /* filth init */
        // select filth type ()
        // load filth model
        // apply convex mesh collider
        /* filth highlight init */
        // dup model mesh as filth highlight
        // set filth highlight to "filth" layer
        // change material to SelectionHighlightMat
        // meshrenderer.enabled = false
    }

    private void Update()
    {

    }
}
