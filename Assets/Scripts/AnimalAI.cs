using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    GameObject animalWaypoint = null;
    
    public void SetWaypointObject(GameObject newWaypoint)
    {
        animalWaypoint = newWaypoint;
    }

    private void Update()
    {
        FollowWaypoint();
    }

    private void FollowWaypoint()
    {
        if (animalWaypoint == null) return;
        this.transform.position = animalWaypoint.transform.position; // placeholder
    }

}
