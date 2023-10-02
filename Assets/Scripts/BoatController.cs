using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    // Player Movement Constants
    private const float rotationSpeed = 2.5f;
    private const float boatSpeed = 5f;
    private const float fuelUseRate = .005f;
    private const float boatSlowdownThreshold = 3f;
    private const float boatStopThreshold = .5f;
    // Basic Player Movement
    private Rigidbody playerBody;
    private Vector3 movementDirection;
    // Click Marker
    private GameObject clickMarker;
    private MeshRenderer clickMarkerRenderer;
    private LineRenderer clickMarkerLine;
    private RaycastHit clickHitInfo;
    private Vector3 clickPosition;
    // Barrier
    private Ray[] barrierRayList;
    private RaycastHit[] barrierHitInfo;
    private float boatBarrierRange = 5f;
    private bool avoidBarrier = false;
    // GUI
    private LevelDisplayController levelDisplayController;
    [SerializeField] private GameObject levelOverOverlay;

    private void Start()
    {
        // barrier init
        barrierRayList = new Ray[(int)BOAT_DIRECTION._COUNT];
        barrierHitInfo = new RaycastHit[(int)BOAT_DIRECTION._COUNT];
        // click marker init
        clickMarker = GameObject.Find("ClickMarker");
        clickMarkerRenderer = clickMarker.GetComponent<MeshRenderer>();
        clickMarkerLine = clickMarker.GetComponent<LineRenderer>();
        clickPosition = Vector3.zero;
        // player init
        playerBody = this.GetComponent<Rigidbody>();
        movementDirection = Vector3.zero;
        LoadCachedPlayer();
        // gui init
        levelDisplayController = GameObject.Find("Canvas").GetComponent<LevelDisplayController>();
        UpdateDisplays();
    }

    private void EndLevel(LEVEL_STATE endState, Color screenColor)
    {
        levelOverOverlay.GetComponent<Image>().color = screenColor;
        levelOverOverlay.SetActive(true);
        Core.SetLevelState(endState);
        playerBody.velocity = Vector3.zero;
        clickMarkerRenderer.enabled = false;
        clickMarkerLine.enabled = false;
    }

    private void Update()
    {
        if (Core.GetLevelState() != LEVEL_STATE.Ongoing) return;
        
        // player input

        if (Input.GetKeyDown(KeyCode.H)) // debug
        {
            this.EndLevel(LEVEL_STATE.Success, new Color(0f, 1f, 0f, .5f));
        }

        if (!avoidBarrier && Input.GetKey(KeyCode.Mouse1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out clickHitInfo, Mathf.Infinity, 1 << (int)LAYERS.Water))
            {
                clickPosition = clickHitInfo.point;
                if (!clickMarkerRenderer.enabled) clickMarkerRenderer.enabled = true;
                if (!clickMarkerLine.enabled) clickMarkerLine.enabled = true;
            }
        }

        // other stuff

        clickMarker.transform.position = clickPosition;
        clickMarker.transform.Rotate(Vector3.up, 360 * Time.deltaTime);
        if (clickMarkerRenderer.enabled && playerBody.velocity.magnitude == 0f)
        {
            clickMarkerRenderer.enabled = false;
            clickMarkerLine.enabled = false;
            avoidBarrier = false;
        }

        movementDirection = (clickPosition - this.transform.position);

        AvoidBarrier();

        UpdateDisplays();

        // level loss check (placeholder)

        float currentFuel = Core.GetPlayerFuel();
        float currentHealth = Core.GetPlayerHealth();
        if (currentFuel <= 0f || currentHealth <= 0f)
        {
            LEVEL_STATE endState = (currentFuel <= 0f && currentHealth > 0f ? LEVEL_STATE.FuelFail : (currentHealth <= 0f && currentFuel > 0f ? LEVEL_STATE.HealthFail : LEVEL_STATE.Stopped));
            Color screenColor = (currentFuel <= 0f && currentHealth > 0f ? new Color(1f, 1f, 0f, .5f) : (currentHealth <= 0f && currentFuel > 0f ? new Color(1f, 0f, 0f, .5f) : new Color(0f, 1f, 1f, .5f)));
            this.EndLevel(endState, screenColor);
            levelOverOverlay.SetActive(true);
        }

        // click marker line renderer

        /*
        int segmentSize = (int)((clickPosition - this.transform.position).magnitude / 10);
        clickMarkerLine.positionCount = 2 + segmentSize;

        for(int positionIndex = 1; positionIndex < clickMarkerLine.positionCount - 1; positionIndex++)
        {
            clickMarkerLine.SetPosition(positionIndex, this.transform.position + (clickPosition - this.transform.position).normalized * segmentSize * positionIndex);
        }*/
        
        clickMarkerLine.SetPosition(0, this.transform.position);
        clickMarkerLine.SetPosition(clickMarkerLine.positionCount - 1, clickPosition);


        // debug stuff

        DebugDrawBarrierCollisionRays();
        Debug.DrawLine(this.transform.position, this.transform.position + movementDirection);
    }

    private void FixedUpdate()
    {
        if (Core.GetLevelState() != LEVEL_STATE.Ongoing) return;

        MovePlayerBoat();
        if (playerBody.velocity.magnitude != 0f)
        {
            Core.IncrementPlayerFuel(-playerBody.velocity.magnitude * fuelUseRate);
        }
    }

    private void AvoidBarrier()
    {
        bool barrierHitFlag = false;
        barrierRayList[(int)BOAT_DIRECTION.Port] = new Ray(this.transform.position, -this.transform.right);
        barrierRayList[(int)BOAT_DIRECTION.Port_Bow] = new Ray(this.transform.position, -this.transform.right + this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Bow] = new Ray(this.transform.position, this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Starboard_Bow] = new Ray(this.transform.position, this.transform.right + this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Starboard] = new Ray(this.transform.position, this.transform.right);
        barrierRayList[(int)BOAT_DIRECTION.Starboard_Stern] = new Ray(this.transform.position, this.transform.right - this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Stern] = new Ray(this.transform.position, - this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Port_Stern] = new Ray(this.transform.position, - this.transform.right - this.transform.forward);

        int hitIndex = 0;
        for (int index = 0; index < (int)BOAT_DIRECTION._COUNT; index++)
        {
            if (!avoidBarrier && Physics.Raycast(barrierRayList[index], out barrierHitInfo[index], boatBarrierRange, 1 << (int)LAYERS.Barrier))
            {
                barrierHitFlag = true;
                avoidBarrier = true;
                break;
            }
        }

        if (!barrierHitFlag) return;

        clickPosition = this.transform.position - barrierRayList[hitIndex].direction * boatBarrierRange;
        clickMarkerRenderer.enabled = true;
        clickMarkerLine.enabled = true;
    }

    private void MovePlayerBoat()
    {
        float speedMultiplier = movementDirection.magnitude <= boatStopThreshold ? 0f : Mathf.Min(movementDirection.magnitude, boatSlowdownThreshold) / boatSlowdownThreshold;
        float currentAngle = Vector3.Angle(this.transform.forward, movementDirection);
        Vector3 currentVelocity = this.transform.forward * (boatSpeed * (currentAngle >= 90 ? .3f : (currentAngle >= 45 ? .6f : 1f)));

        if (currentAngle > 5f && movementDirection.magnitude > boatStopThreshold) this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * rotationSpeed);

        playerBody.velocity = currentVelocity * speedMultiplier;
    }

    private bool LoadCachedPlayer()
    {
        Core.IncrementPlayerHealth(-40); // debug
        if (!Core.IsPlayerLoadStaged()) return false;
        this.transform.position = Core.GetPlayerPosition();
        clickPosition = this.transform.position;
        this.transform.rotation = Core.GetPlayerRotation();
        return true;
    }

    private void UpdateDisplays()
    {
        levelDisplayController.UpdateFuelDisplay();
        levelDisplayController.UpdateHealthDisplay();
        levelDisplayController.UpdateGoldDisplay();
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
