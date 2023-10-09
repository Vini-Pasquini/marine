using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerBoat;
    private bool lockOnPlayer;

    private bool movingCamera;
    private Vector3 clickPosition;
    private Vector3 anchorPosition;
    private float cameraSpeed = .015f;

    [SerializeField] private GameObject boatLocator;

    [SerializeField] private GameObject enemyLocator;
    private GameObject enemyLocatorTarget = null;
    public GameObject GetEnemyLocatorTarget() { return enemyLocatorTarget; }
    public void SetEnemyLocatorTarget(GameObject newTarget) { enemyLocatorTarget = newTarget; }

    [SerializeField] private GameObject animalLocator;
    private GameObject animalLocatorTarget = null;
    public GameObject GetAnimalLocatorTarget() { return animalLocatorTarget; }
    public void SetAnimalLocatorTarget(GameObject newTarget) { animalLocatorTarget = newTarget; }

    [SerializeField] private GameObject fuelLocator;
    private GameObject fuelLocatorTarget = null;
    public GameObject GetFuelLocatorTarget() { return fuelLocatorTarget; }
    public void SetFuelLocatorTarget(GameObject newTarget) { fuelLocatorTarget = newTarget; }

    [SerializeField] private GameObject interactionMenu;
    private Transform interactionMenuGrid;

    private float interactionDistance = 0f;

    private void Start()
    {
        lockOnPlayer = true;
        clickPosition = Vector3.zero;

        //interactionMenu = GameObject.Find("InteractionMenu");
        interactionMenuGrid = interactionMenu.transform.GetChild(0);
        interactionDistance = Core.GetRadarRange();
    }

    RaycastHit hitInfo;
    GameObject hoveredObject;
    private GameObject cachedHoverdObject;

    public GameObject GetCachedHoveredObject()
    {
        return cachedHoverdObject;
    }

    private float scrollSpeed = 1000f; // placeholder
    private float minCameraDistance = 10f; // placeholder
    private float maxCameraDistance = 100f; // placeholder


    private void Update()
    {
        if (Core.GetLevelState() != LEVEL_STATE.Ongoing) return;

        float cameraRigScale = this.transform.localScale.x;
        float cameraRigScrollDelta = (Input.mouseScrollDelta.y * Time.deltaTime) * scrollSpeed;
        cameraRigScale = Mathf.Max(minCameraDistance, Mathf.Min(maxCameraDistance, cameraRigScale - cameraRigScrollDelta));
        this.transform.localScale = Vector3.one * cameraRigScale;

        if (Input.GetKeyDown(KeyCode.F))
        {
            lockOnPlayer = !lockOnPlayer;
        }

        ObjectHighlight(); // placeholder

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // camera control
            movingCamera = true;
            clickPosition = Input.mousePosition;
            anchorPosition = this.transform.position;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            // camera control
            movingCamera = false;
            anchorPosition.y = 0f;
            // object interaction
            InteractionHandler();
        }

        cameraSpeed = cameraRigScale / 1500f;

        if (!lockOnPlayer)
        {
            if (movingCamera)
            {
                Vector3 cameraMovementDelta = clickPosition - Input.mousePosition;
                Vector3 currentPosition = this.transform.position;
                currentPosition.x = (cameraMovementDelta.x + cameraMovementDelta.y) * (cameraSpeed);
                currentPosition.y = 0f;
                currentPosition.z = (cameraMovementDelta.y - cameraMovementDelta.x) * (cameraSpeed);
                this.transform.localPosition = anchorPosition + currentPosition;
            }
        }
        else
        {
            this.transform.position = playerBoat.transform.position;
        }

        UpdateLocator(boatLocator, playerBoat);
        UpdateLocator(enemyLocator, enemyLocatorTarget, true);
        UpdateLocator(animalLocator, animalLocatorTarget, true);
        UpdateLocator(fuelLocator, fuelLocatorTarget, true);
    }

    private void InteractionHandler()
    {
        foreach (Button childObject in interactionMenuGrid.GetComponentsInChildren<Button>()) childObject.gameObject.SetActive(false);
        if (cachedHoverdObject == null) return;
        switch (cachedHoverdObject.layer)
        {
            case (int)LAYERS.Enemy:
                interactionMenuGrid.Find("Battle").gameObject.SetActive(true);
                break;
            case (int)LAYERS.Animal:
                AnimalController animalController = cachedHoverdObject.GetComponentInParent<AnimalController>();
                if (animalController == null) break;
                if (animalController.GetIsSafe()) break;

                Transform rescueButton = interactionMenuGrid.Find("Rescue"); ;
                if (!Core.GetIsOnRescue())
                {
                    rescueButton.gameObject.SetActive(true);
                    rescueButton.GetComponent<Button>().interactable = true;
                    break;
                }
                if (!animalController.IsFollowingBoat())
                {
                    rescueButton.gameObject.SetActive(true);
                    rescueButton.GetComponent<Button>().interactable = false;
                    break;
                }
                interactionMenuGrid.Find("Release").gameObject.SetActive(true);
                break;

                //interactionMenuGrid.Find(Core.GetIsOnRescue() ? "Release" : "Rescue").gameObject.SetActive(true);
                break;
            default: break;
        }
        interactionMenuGrid.Find("Mark").gameObject.SetActive(true);
        interactionMenu.transform.position = Input.mousePosition;
        interactionMenu.SetActive(!interactionMenu.activeSelf && hoveredObject != null && (hoveredObject.transform.position - playerBoat.transform.position).magnitude <= interactionDistance);
    }

    [SerializeField] private LayerMask layerMask;

    // placeholder
    private void ObjectHighlight()
    {
        hoveredObject = null;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
        {
            hoveredObject = hitInfo.collider.gameObject;
        }
        if (hoveredObject == null)
        {
            if (cachedHoverdObject != null) cachedHoverdObject.GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        cachedHoverdObject = hoveredObject;
        cachedHoverdObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private bool UpdateLocator(GameObject locator, GameObject target, bool markInsideViewport = false)
    {
        // test locator flip fix
        /* EU PRECISO CORRIGIR ISSO ANTES DO PLAYTEST; SOCORRO!!!
        Vector3 treco = playerBoat.transform.position - Camera.main.transform.position;
        float A = Camera.main.transform.forward.x * (Camera.main.transform.up.x - treco.x);
        float B = Camera.main.transform.forward.y * (Camera.main.transform.up.y - treco.y);
        float C = Camera.main.transform.forward.y * (Camera.main.transform.up.z - treco.z);
        if (A + B + C > 0)
        {
            Debug.Log("Has Flip");
        }
        */

        if (target == null)
        {
            if (locator.activeSelf) locator.SetActive(false);
            return false;
        }

        Vector3 targetViewportPosition = Camera.main.WorldToViewportPoint(target.transform.position);
        Vector3 newLocatorViewportPosition = new Vector3
        (
            newLocatorViewportPosition.x = Mathf.Min(1f, Mathf.Max(0f, targetViewportPosition.x)),
            newLocatorViewportPosition.y = Mathf.Min(1f, Mathf.Max(0f, targetViewportPosition.y)),
            0f
        );
        locator.transform.position = Camera.main.ViewportToScreenPoint(newLocatorViewportPosition);

        if (targetViewportPosition.x <= 0f || targetViewportPosition.x >= 1f || targetViewportPosition.y <= 0f || targetViewportPosition.y >= 1f || markInsideViewport)
        {
            if (!locator.activeSelf) locator.SetActive(true);
            Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(.5f, .5f, 0f));
            Vector3 screenUp = Camera.main.ViewportToScreenPoint(new Vector3(0f, .5f, 0f));
            Vector3 boatScreenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            locator.transform.rotation = Quaternion.Euler(0f, 0f, (Vector3.Angle(screenUp, boatScreenPosition - screenCenter)) * (targetViewportPosition.x > .5f ? -1 : 1));
            return true;
        }
        if (locator.activeSelf) locator.SetActive(false);
        return false;
    }
}
