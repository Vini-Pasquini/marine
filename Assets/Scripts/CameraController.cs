using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerBoat;
    private bool lockOnPlayer;

    private bool movingCamera;
    private Vector3 clickPosition;
    private Vector3 anchorPosition;
    private float cameraSpeed = .015f;

    [SerializeField] private GameObject boatLocator;
    private Image boatLocatorImage;


    private void Start()
    {
        lockOnPlayer = true;
        clickPosition = Vector3.zero;
        boatLocatorImage = boatLocator.GetComponent<Image>();
        boatLocatorImage.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lockOnPlayer = !lockOnPlayer;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            movingCamera = true;
            clickPosition = Input.mousePosition;
            anchorPosition = this.transform.position;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
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

        LocateBoat();
    }

    private void LocateBoat()
    {
        Vector3 boatViewportPosition = Camera.main.WorldToViewportPoint(playerBoat.transform.position);
        boatViewportPosition.z = 0f;
        float rotationAngle = Vector3.Angle(Vector3.up, boatViewportPosition - (Vector3.one * .5f));
        if (boatViewportPosition.x <= 0f || boatViewportPosition.x >= 1f || boatViewportPosition.y <= 0f || boatViewportPosition.y >= 1f)
        {
            boatLocatorImage.enabled = true;
            boatLocator.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle * (boatViewportPosition.x > .5f ? -1 : 1));
        }
        else
        {
            boatLocatorImage.enabled = false;
        }
    }
}
