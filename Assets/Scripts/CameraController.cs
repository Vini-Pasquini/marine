using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;
using UnityEngine.UI;
using System.Linq;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerBoat;
    private bool lockOnPlayer;

    private bool movingCamera;
    private Vector3 clickPosition;
    private Vector3 anchorPosition;
    private float cameraSpeed = .015f;

    [SerializeField] private GameObject boatLocator;

    private void Start()
    {
        lockOnPlayer = true;
        clickPosition = Vector3.zero;
        boatLocator.SetActive(false);
        
        
    }

    RaycastHit hitInfo;
    GameObject hoveredObject;
    GameObject cashedHoverdObject;

    [SerializeField] private GameObject interactionMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lockOnPlayer = !lockOnPlayer;
        }

        EnemyHighlight(); // placeholder

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            movingCamera = true;
            clickPosition = Input.mousePosition;
            anchorPosition = this.transform.position;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            interactionMenu.transform.position = Input.mousePosition;
            interactionMenu.SetActive(hoveredObject != null);
            movingCamera = false;
            anchorPosition.y = 0f;
        }

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

    // placeholder
    private void EnemyHighlight()
    {
        hoveredObject = null;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1 << (int)LAYERS.Enemy))
        {
            hoveredObject = hitInfo.transform.gameObject;
        }
        if (hoveredObject == null)
        {
            if (cashedHoverdObject != null) cashedHoverdObject.GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        cashedHoverdObject = hoveredObject;
        cashedHoverdObject.GetComponent<MeshRenderer>().enabled = true;
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
