using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    GameObject animalWaypoint;
    AnimalAI animal;
    GameObject targetObject = null;

    private void Start()
    {
        /* animal init */
        // select animal type ()
        // load animal model
        // apply convex mesh collider
        /* animal highlight init */
        // dup model mesh as animal highlight
        // set animal highlight to "animal" layer
        // change material to SelectionHighlightMat
        // meshrenderer.enabled = false
        /*  */
        animalWaypoint = this.transform.GetChild(0).gameObject;
        animal = this.transform.GetChild(1).GetComponent<AnimalAI>();
        animal.SetWaypointObject(animalWaypoint);
    }

    private void Update()
    {
        TrackTargetObject();
    }

    private void TrackTargetObject()
    {
        if (targetObject == null) return;
        animalWaypoint.transform.position = targetObject.transform.position;
    }

    public void SetTargetObject(GameObject newtarget)
    {
        targetObject = newtarget;
    }
}
