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

    private GameObject clickMarker;

    [SerializeField] private Material MarkerMat;

    private void Start()
    {
        playerBody = this.GetComponent<Rigidbody>();
        movementDirection = Vector3.zero;
        clickPosition = Vector3.zero;
        clickMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        clickMarker.GetComponent<SphereCollider>().enabled = false;
        clickMarker.GetComponent<MeshRenderer>().material = MarkerMat;
        clickMarker.SetActive(false);
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
                if (!clickMarker.activeSelf) clickMarker.SetActive(true);
            }
        }

        clickMarker.transform.Rotate(Vector3.up, 360 * Time.deltaTime);
        if (clickMarker.activeSelf && this.transform.position == clickPosition) clickMarker.SetActive(true);

        movementDirection = (clickPosition - this.transform.position);
    }

    private void FixedUpdate()
    {
        if (movementDirection.magnitude > 1f)
        {
            float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
            playerBody.velocity = this.transform.forward * (boatSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));
            if (currentAngle > 1f)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationSpeed);
            }
        }
    }
}
