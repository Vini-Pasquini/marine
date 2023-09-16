using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    RaycastHit clickHitInfo;
    RaycastHit[] barrierHitInfo;

    private void Start()
    {
        barrierHitInfo = new RaycastHit[8];
        playerBody = this.GetComponent<Rigidbody>();
        movementDirection = Vector3.zero;
        clickPosition = Vector3.zero;
        clickMarkerRenderer = clickMarker.GetComponent<MeshRenderer>();
    }

    [SerializeField] LevelDisplayController levelDisplayController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Core.SetPlayerFuel(1);
            Core.SetPlayerHealth(2);
            Core.SetPlayerGold(3);
            levelDisplayController.UpdateFuelDisplay();
            levelDisplayController.UpdateHealthDisplay();
            levelDisplayController.UpdateGoldDisplay();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out clickHitInfo, Mathf.Infinity, 1 << (int)LAYERS.Water))
            {
                clickPosition = clickHitInfo.point;
                clickMarker.transform.position = clickPosition;
                if (!clickMarkerRenderer.enabled) clickMarkerRenderer.enabled = true;
            }
        }

        clickMarker.transform.Rotate(Vector3.up, 360 * Time.deltaTime);
        if (clickMarkerRenderer.enabled && playerBody.velocity.magnitude == 0f) clickMarkerRenderer.enabled = false;

        movementDirection = (clickPosition - this.transform.position);

        //AvoidBarrier();

        Debug.DrawLine(this.transform.position, this.transform.position + movementDirection);
    }

    private float boatBarrierRange = 5f;

    private void AvoidBarrier()
    {
        bool barrierHitFlag = false;
        Ray[] rayList =
        {
            new Ray(this.transform.position, -this.transform.right),
            new Ray(this.transform.position, -this.transform.right + this.transform.forward),
            new Ray(this.transform.position, this.transform.forward),
            new Ray(this.transform.position, this.transform.right + this.transform.forward),
            new Ray(this.transform.position, this.transform.right),
            new Ray(this.transform.position, this.transform.right - this.transform.forward),
            new Ray(this.transform.position, - this.transform.forward),
            new Ray(this.transform.position, - this.transform.right - this.transform.forward),
        };

        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right + this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right + this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right - this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right - this.transform.forward).normalized * boatBarrierRange);

        for (int index = 0; index < 8; index++)
        {
            if (Physics.Raycast(rayList[index], out barrierHitInfo[index], boatBarrierRange, 1 << (int)LAYERS.Barrier)) barrierHitFlag = true;
        }

        if (!barrierHitFlag) return;
        
        /*
         *
         * rotate boat to avoid barrier here
         *
         */
    }

    private void FixedUpdate()
    {
        MovePlayerBoat();
    }

    private float boatSlowdownThreshold = 3f;
    private float boatStopThreshold = .5f;

    private void MovePlayerBoat()
    {
        float speedMultiplier = movementDirection.magnitude <= boatStopThreshold ? 0f : Mathf.Min(movementDirection.magnitude, boatSlowdownThreshold) / boatSlowdownThreshold;
        float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
        Vector3 currentVelocity = this.transform.forward * (boatSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));
        
        if (currentAngle > 5f && movementDirection.magnitude > boatStopThreshold) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationSpeed);

        playerBody.velocity = currentVelocity * speedMultiplier;
    }
}
