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
    
    [SerializeField] private GameObject interactionMenu;
    private Transform interactionMenuGrid;

    private float interactionDistance = 0f;

    private void Start()
    {
        lockOnPlayer = true;
        clickPosition = Vector3.zero;
        boatLocator.SetActive(false);

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
                if (SafeAnimal()) break;
                interactionMenuGrid.Find(Core.GetIsOnRescue() ? "Release" : "Rescue").gameObject.SetActive(true);
                break;
            default: break;
        }
        interactionMenuGrid.Find("Mark").gameObject.SetActive(true);
        interactionMenu.transform.position = Input.mousePosition;
        interactionMenu.SetActive(!interactionMenu.activeSelf && hoveredObject != null && (hoveredObject.transform.position - playerBoat.transform.position).magnitude <= interactionDistance);
    }

    private bool SafeAnimal()
    {
        /* placeholder */
        AnimalController animalController = cachedHoverdObject.GetComponentInParent<AnimalController>();
        if (animalController == null) return false;
        return animalController.GetIsSafe();
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

    private void UpdateLocator(GameObject locator, GameObject target)
    {
        // test locator flip fix
        /*
        Vector3 treco = playerBoat.transform.position - Camera.main.transform.position;
        float A = Camera.main.transform.forward.x * (Camera.main.transform.up.x - treco.x);
        float B = Camera.main.transform.forward.y * (Camera.main.transform.up.y - treco.y);
        float C = Camera.main.transform.forward.y * (Camera.main.transform.up.z - treco.z);
        if (A + B + C > 0)
        {
            Debug.Log("Has Flip");
        }
        */

        Vector3 boatViewportPosition = Camera.main.WorldToViewportPoint(target.transform.position);
        Vector3 newLocatorViewportPosition = new Vector3
        (
            newLocatorViewportPosition.x = Mathf.Min(1f, Mathf.Max(0f, boatViewportPosition.x)),
            newLocatorViewportPosition.y = Mathf.Min(1f, Mathf.Max(0f, boatViewportPosition.y)),
            0f
        );
        locator.transform.position = Camera.main.ViewportToScreenPoint(newLocatorViewportPosition);

        if (boatViewportPosition.x <= 0f || boatViewportPosition.x >= 1f || boatViewportPosition.y <= 0f || boatViewportPosition.y >= 1f)
        {
            if (!locator.activeSelf) locator.SetActive(true);
            Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(.5f, .5f, 0f));
            Vector3 screenUp = Camera.main.ViewportToScreenPoint(new Vector3(0f, .5f, 0f));
            Vector3 boatScreenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            locator.transform.rotation = Quaternion.Euler(0f, 0f, (Vector3.Angle(screenUp, boatScreenPosition - screenCenter)) * (boatViewportPosition.x > .5f ? -1 : 1));
            return;
        }
        if (locator.activeSelf) locator.SetActive(false);
    }
}
