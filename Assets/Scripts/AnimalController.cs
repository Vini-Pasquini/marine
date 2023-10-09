using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    GameObject animalWaypoint;
    AnimalAI animal;
    GameObject targetObject = null;

    public bool IsFollowingBoat()
    {
        return targetObject != null;
    }

    private float baseAnimalSpeed = 5f;

    private bool isSafe = false;

    public bool GetIsSafe()
    {
        return isSafe;
    }

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
        animal.SetAnimalSpeed(baseAnimalSpeed);
        animal.SetAnimalRotationRate(baseAnimalSpeed * .1f);
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

    public void SetTargetObject(GameObject newTarget)
    {
        animal.SetAnimalSpeed(newTarget != null ? baseAnimalSpeed * 3 : baseAnimalSpeed);
        animal.SetAnimalRotationRate(newTarget != null ? baseAnimalSpeed * .3f : baseAnimalSpeed * .1f);
        RaycastHit hit;
        if (newTarget == null && Physics.Raycast((animalWaypoint.transform.position + Vector3.up), Vector3.down, out hit, Mathf.Infinity, 1 << (int)LAYERS.RescueArea))
        {
            isSafe = true;
            Core.IncrementPlayerGold(10);
            Core.IncrementAnimalCount(-1);
        }
        targetObject = newTarget;
    }
}
