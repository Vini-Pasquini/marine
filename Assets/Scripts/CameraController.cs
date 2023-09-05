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

    // LocateBoat()
    Vector3 newBoatLocatorViewportPosition = Vector3.zero;
    Vector3 screenCenter;
    Vector3 screenUp;

    private void Start()
    {
        lockOnPlayer = true;
        clickPosition = Vector3.zero;
        boatLocatorImage = boatLocator.GetComponent<Image>();
        boatLocatorImage.enabled = false;
        screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(.5f, .5f, 0f)); // const
        screenUp = Camera.main.ViewportToScreenPoint(new Vector3(0f, .5f, 0f)); // const
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

        Vector3 boatViewportPosition = Camera.main.WorldToViewportPoint(playerBoat.transform.position);
        Vector3 boatScreenPosition = Camera.main.WorldToScreenPoint(playerBoat.transform.position);
        // placeholder - not 100% accurate
        newBoatLocatorViewportPosition.x = Mathf.Min(1f, Mathf.Max(0f, boatViewportPosition.x));
        newBoatLocatorViewportPosition.y = Mathf.Min(1f, Mathf.Max(0f, boatViewportPosition.y));
        boatLocator.transform.position = Camera.main.ViewportToScreenPoint(newBoatLocatorViewportPosition);

        if (boatViewportPosition.x <= 0f || boatViewportPosition.x >= 1f || boatViewportPosition.y <= 0f || boatViewportPosition.y >= 1f)
        {
            boatLocatorImage.enabled = true;
            boatLocator.transform.rotation = Quaternion.Euler(0f, 0f, (Vector3.Angle(screenUp, boatScreenPosition - screenCenter)) * (boatViewportPosition.x > .5f ? -1 : 1));
        }
        else
        {
            boatLocatorImage.enabled = false;
        }

        Debug.DrawLine(screenCenter, boatScreenPosition);
        Debug.DrawLine(playerBoat.transform.position, boatScreenPosition);
    }
}
