using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    private GameObject animalWaypoint = null;
    private Rigidbody animalBody;

    private float animalSpeed = 0f;
    private float rotationRate = 0f; // always 10% of animalSpeed

    public void SetWaypointObject(GameObject newWaypoint)
    {
        animalWaypoint = newWaypoint;
    }

    public void SetAnimalSpeed(float newSpeed)
    {
        animalSpeed = newSpeed;
    }

    public void SetAnimalRotationRate(float newRate)
    {
        rotationRate = newRate;
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

        Vector3 waypointPosition = animalWaypoint.transform.position;
        waypointPosition.y = -1f;
        Vector3 movementDirection = waypointPosition - this.transform.position;

        float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
        Vector3 currentVelocity = this.transform.forward * (animalSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));

        if (currentAngle > 10f) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationRate);

        animalBody.velocity = currentVelocity;
    }

}
