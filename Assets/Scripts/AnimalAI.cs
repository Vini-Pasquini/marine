using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    GameObject animalWaypoint = null;
    Rigidbody animalBody;

    public void SetWaypointObject(GameObject newWaypoint)
    {
        animalWaypoint = newWaypoint;
    }

    private void Start()
    {
        animalBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FollowWaypoint();
    }

    private void FollowWaypoint()
    {
        if (animalWaypoint == null) return;
        // this.transform.position = animalWaypoint.transform.position; // placeholder

        Vector3 movementDirection = animalWaypoint.transform.position - this.transform.position;
        float animalSpeed = 15f;
        float rotationSpeed = 1.5f;

        float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
        Vector3 currentVelocity = this.transform.forward * (animalSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));

        if (currentAngle > 10f) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationSpeed);

        animalBody.velocity = currentVelocity;
    }

}
