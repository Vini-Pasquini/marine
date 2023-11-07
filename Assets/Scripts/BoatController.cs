using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
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

    private GameObject playerSpawn;
    private LevelManager levelManeger;
    private GameObject radarThingy;

    private void Start()
    {
        radarThingy = GameObject.Find("Radar"); // ph
        radarThingy.transform.localScale = ((Vector3.forward + Vector3.right) * (Core.GetRadarRange() / 5 /*plane default radius*/)) + Vector3.up;
        // spawn
        playerSpawn = GameObject.Find("PlayerSpawn");
        levelManeger = playerSpawn.GetComponent<LevelManager>();
        // barrier init
        barrierRayList = new Ray[(int)BOAT_DIRECTION._COUNT];
        barrierHitInfo = new RaycastHit[(int)BOAT_DIRECTION._COUNT];
        // click marker init
        clickMarker = GameObject.Find("ClickMarker");
        clickMarkerRenderer = clickMarker.GetComponent<MeshRenderer>();
        clickMarkerLine = clickMarker.GetComponent<LineRenderer>();
        // player init
        playerBody = this.GetComponent<Rigidbody>();
        movementDirection = Vector3.zero;
        LoadCachedPlayer();
        // gui init
        Core.UpdateDisplays();
    }

    public void EndLevel(LEVEL_STATE endState, Color screenColor) // ph
    {
        playerBody.velocity = Vector3.zero;
        clickMarkerRenderer.enabled = false;
        clickMarkerLine.enabled = false;
        levelManeger.DisplayLevelOverScreen(endState, screenColor);
    }

    Vector3 seaLevelPosition = Vector3.zero;
    Vector3 seaLevelRotation = Vector3.zero;

    private void KeepAtSeeLevel()
    {
        // cap position
        seaLevelPosition = this.transform.position;
        seaLevelPosition.y = 0;
        this.transform.position = seaLevelPosition;
        // cap rotation
        seaLevelRotation = this.transform.rotation.eulerAngles;
        seaLevelRotation.x = 0;
        seaLevelRotation.z = 0;
        this.transform.rotation = Quaternion.Euler(seaLevelRotation);
    }

    Vector3 bufferDirection = Vector3.zero;

    private void Update()
    {
        if (Core.GetLevelState() != LEVEL_STATE.Ongoing) return;
        
        // player input

        if (Core.GetAnimalCount() <= 0f && Core.GetEnemyCount() <= 0f) // debug
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

        if (!avoidBarrier && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift)))
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                bufferDirection = Vector3.zero;
                bufferDirection.y += Input.GetKey(KeyCode.W) ? 1 : 0;
                bufferDirection.x += Input.GetKey(KeyCode.A) ? -1 : 0;
                bufferDirection.y += Input.GetKey(KeyCode.S) ? -1 : 0;
                bufferDirection.x += Input.GetKey(KeyCode.D) ? 1 : 0;
                bufferDirection = bufferDirection.normalized * boatBarrierRange;
            }

            clickPosition.x = this.transform.position.x + (bufferDirection.x + bufferDirection.y);
            clickPosition.y = 0f;
            clickPosition.z = this.transform.position.z + (bufferDirection.y - bufferDirection.x);

            if (!clickMarkerRenderer.enabled) clickMarkerRenderer.enabled = true;
            if (!clickMarkerLine.enabled) clickMarkerLine.enabled = true;
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
        KeepAtSeeLevel();

        // level loss check (placeholder)

        float currentFuel = Core.GetPlayerFuel();
        float currentHealth = Core.GetPlayerHealth();
        if (currentFuel <= 0f || currentHealth <= 0f)
        {
            LEVEL_STATE endState = (currentFuel <= 0f && currentHealth > 0f ? LEVEL_STATE.FuelFail : (currentHealth <= 0f && currentFuel > 0f ? LEVEL_STATE.HealthFail : LEVEL_STATE.Stopped));
            Color screenColor = (currentFuel <= 0f && currentHealth > 0f ? new Color(1f, 1f, 0f, .5f) : (currentHealth <= 0f && currentFuel > 0f ? new Color(1f, 0f, 0f, .5f) : new Color(0f, 1f, 1f, .5f)));
            this.EndLevel(endState, screenColor);
        }

        // click marker line renderer
        
        /*
        int segmentAmount = 100;
        int segmentMaxHeight = 20;
        float segmentSize = (clickPosition - this.transform.position).magnitude / segmentAmount;
        clickMarkerLine.positionCount = 1 + segmentAmount;

        for(int positionIndex = 1; positionIndex < clickMarkerLine.positionCount - 1; positionIndex++)
        {
            Vector3 segmentPosition = this.transform.position + (clickPosition - this.transform.position).normalized * segmentSize * positionIndex;
            segmentPosition.y = segmentPosition.y + (segmentMaxHeight * (1 - (Mathf.Abs(positionIndex - (segmentAmount / 2)) / (segmentAmount/2))));
            clickMarkerLine.SetPosition(positionIndex, segmentPosition);
        }
        */

        clickMarkerLine.SetPosition(0, this.transform.position);
        clickMarkerLine.SetPosition(clickMarkerLine.positionCount - 1, clickPosition);
        
        radarRotation.y += 180 * Time.deltaTime;
        radarThingy.transform.rotation = Quaternion.Euler(radarRotation);

        // debug stuff

        DebugDrawBarrierCollisionRays();
        Debug.DrawLine(this.transform.position, this.transform.position + movementDirection);
    }

    Vector3 radarRotation = Vector3.zero;

    float currentScale = 0f;

    private void FixedUpdate()
    {
        if (Core.GetLevelState() != LEVEL_STATE.Ongoing) return;

        MovePlayerBoat();
        if (playerBody.velocity.magnitude != 0f)
        {
            Core.IncrementPlayerFuel(-playerBody.velocity.magnitude * fuelUseRate);
        }
    }

    private int debugCollidingRayIndex = -1;

    private void AvoidBarrier()
    {
        bool barrierHitFlag = false;
        barrierRayList[(int)BOAT_DIRECTION.Port] = new Ray(this.transform.position, (-this.transform.right));
        barrierRayList[(int)BOAT_DIRECTION.Port_Bow] = new Ray(this.transform.position, (-this.transform.right) + (this.transform.forward));
        barrierRayList[(int)BOAT_DIRECTION.Bow] = new Ray(this.transform.position, this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Starboard_Bow] = new Ray(this.transform.position, (this.transform.right) + (this.transform.forward));
        barrierRayList[(int)BOAT_DIRECTION.Starboard] = new Ray(this.transform.position, (this.transform.right));
        barrierRayList[(int)BOAT_DIRECTION.Starboard_Stern] = new Ray(this.transform.position, (this.transform.right) - (this.transform.forward));
        barrierRayList[(int)BOAT_DIRECTION.Stern] = new Ray(this.transform.position, -this.transform.forward);
        barrierRayList[(int)BOAT_DIRECTION.Port_Stern] = new Ray(this.transform.position, (-this.transform.right) - (this.transform.forward));

        int hitIndex = 0;
        for (int index = 0; index < (int)BOAT_DIRECTION._COUNT; index++)
        {
            if (!avoidBarrier && Physics.Raycast(barrierRayList[index], out barrierHitInfo[index], boatBarrierRange, 1 << (int)LAYERS.Barrier))
            {
                barrierHitFlag = true;
                avoidBarrier = true;
                hitIndex = index;
                debugCollidingRayIndex = index;
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
        //Core.IncrementPlayerHealth(-40); // debug
        bool cacheFlag = Core.IsPlayerLoadStaged();
        PlayerCache playerCache = Core.GetPlayerCache();
        this.transform.position = cacheFlag ? playerCache.playerPosition : playerSpawn.transform.position;
        clickPosition = this.transform.position;
        this.transform.rotation = cacheFlag ? playerCache.playerRotation : playerSpawn.transform.rotation;
        Core.StagePlayerLoad(false);
        return cacheFlag;
    }

    /* Debug Methods */

    private void DebugDrawBarrierCollisionRays()
    {
        if (!avoidBarrier) debugCollidingRayIndex = -1;
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right).normalized * boatBarrierRange, debugCollidingRayIndex == 0 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right + this.transform.forward).normalized * boatBarrierRange, debugCollidingRayIndex == 1 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.forward).normalized * boatBarrierRange, debugCollidingRayIndex == 2 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right + this.transform.forward).normalized * boatBarrierRange, debugCollidingRayIndex == 3 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right).normalized * boatBarrierRange, debugCollidingRayIndex == 4 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.right - this.transform.forward).normalized * boatBarrierRange, debugCollidingRayIndex == 5 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.forward).normalized * boatBarrierRange, debugCollidingRayIndex == 6 ? Color.red : Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + (-this.transform.right - this.transform.forward).normalized * boatBarrierRange, debugCollidingRayIndex == 7 ? Color.red : Color.green);
    }
}
