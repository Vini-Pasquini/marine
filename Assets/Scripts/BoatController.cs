using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private LevelDisplayController levelDisplayController;
    [SerializeField] private GameObject clickMarker;
    private Rigidbody playerBody;
    private Vector3 movementDirection;
    private Vector3 clickPosition;
    private MeshRenderer clickMarkerRenderer;
    private RaycastHit clickHitInfo;
    private RaycastHit[] barrierHitInfo;
    private float boatSpeed = 5f;
    private float rotationSpeed = 2.5f;
    private float boatSlowdownThreshold = 3f;
    private float boatStopThreshold = .5f;
    private float boatBarrierRange = 5f;
    private bool avoidBarrier = false;

    private void Start()
    {
        barrierHitInfo = new RaycastHit[8];
        playerBody = this.GetComponent<Rigidbody>();
        movementDirection = Vector3.zero;
        clickPosition = Vector3.zero;
        clickMarkerRenderer = clickMarker.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // player input

        if (Input.GetKeyDown(KeyCode.H))
        {
            Core.SetPlayerFuel(1);
            Core.SetPlayerHealth(2);
            Core.SetPlayerGold(3);
            levelDisplayController.UpdateFuelDisplay();
            levelDisplayController.UpdateHealthDisplay();
            levelDisplayController.UpdateGoldDisplay();
        }

        if (!avoidBarrier && Input.GetKey(KeyCode.Mouse1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out clickHitInfo, Mathf.Infinity, 1 << (int)LAYERS.Water))
            {
                clickPosition = clickHitInfo.point;
                if (!clickMarkerRenderer.enabled) clickMarkerRenderer.enabled = true;
            }
        }

        // other stuff

        clickMarker.transform.position = clickPosition;
        clickMarker.transform.Rotate(Vector3.up, 360 * Time.deltaTime);
        if (clickMarkerRenderer.enabled && playerBody.velocity.magnitude == 0f)
        {
            clickMarkerRenderer.enabled = false;
            avoidBarrier = false;
        }

        movementDirection = (clickPosition - this.transform.position);

        AvoidBarrier();

        // debug stuff

        DebugDrawBarrierCollisionRays();
        Debug.DrawLine(this.transform.position, this.transform.position + movementDirection);
    }

    private void FixedUpdate()
    {
        MovePlayerBoat();
    }

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

        int hitIndex = 0;
        for (int index = 0; index < 8; index++)
        {
            if (!avoidBarrier && Physics.Raycast(rayList[index], out barrierHitInfo[index], boatBarrierRange, 1 << (int)LAYERS.Barrier))
            {
                barrierHitFlag = true;
                avoidBarrier = true;
                break;
            }
        }

        if (!barrierHitFlag) return;

        clickPosition = this.transform.position - rayList[hitIndex].direction * boatBarrierRange;
        clickMarkerRenderer.enabled = true;
    }

    private void MovePlayerBoat()
    {
        float speedMultiplier = movementDirection.magnitude <= boatStopThreshold ? 0f : Mathf.Min(movementDirection.magnitude, boatSlowdownThreshold) / boatSlowdownThreshold;
        float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
        Vector3 currentVelocity = this.transform.forward * (boatSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));

        if (currentAngle > 5f && movementDirection.magnitude > boatStopThreshold) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationSpeed);

        playerBody.velocity = currentVelocity * speedMultiplier;
    }

    /* Debug Methods */

    private void DebugDrawBarrierCollisionRays()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right + this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right + this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right - this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.forward).normalized * boatBarrierRange);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right - this.transform.forward).normalized * boatBarrierRange);
    }
}
