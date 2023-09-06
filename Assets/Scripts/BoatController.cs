using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private Rigidbody playerBody;
    Vector3 movementDirection;
    Vector3 clickPosition;
    private float boatSpeed = 5f;
    private float rotationSpeed = 2.5f;

    [SerializeField] private GameObject clickMarker;
    private MeshRenderer clickMarkerRenderer;

    private void Start()
    {
        playerBody = this.GetComponent<Rigidbody>();
        movementDirection = Vector3.zero;
        clickPosition = Vector3.zero;
        clickMarkerRenderer = clickMarker.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.collider.CompareTag("Water"))
            {
                clickPosition = hitInfo.point;
                clickMarker.transform.position = clickPosition;
                if (!clickMarkerRenderer.enabled) clickMarkerRenderer.enabled = true;
            }
        }

        clickMarker.transform.Rotate(Vector3.up, 360 * Time.deltaTime);
        if (clickMarkerRenderer.enabled && playerBody.velocity.magnitude == 0f) clickMarkerRenderer.enabled = false;

        movementDirection = (clickPosition - this.transform.position);
    }

    private void FixedUpdate()
    {
        MovePlayerBoat();
    }

    private void MovePlayerBoat()
    {
        float speedMultiplier = Mathf.Min(movementDirection.magnitude, 1f);
        if (movementDirection.magnitude <= .5f) speedMultiplier = 0f;
        float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
        Vector3 currentVelocity = this.transform.forward * (boatSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));
        if (currentAngle > 5f && speedMultiplier > 0f) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationSpeed);
        playerBody.velocity = currentVelocity * speedMultiplier;
    }
}
