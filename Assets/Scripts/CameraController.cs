using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerBoat;
    private bool lockOnPlayer;

    private bool movingCamera;
    private Vector3 clickPosition;
    private Vector3 anchorPosition;
    private float cameraSpeed = .2f;


    private void Start()
    {
        lockOnPlayer = true;
        clickPosition = Vector3.zero;
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
                currentPosition.x = (cameraMovementDelta.x + cameraMovementDelta.y) * cameraSpeed;
                currentPosition.y = 0f;
                currentPosition.z = (cameraMovementDelta.y - cameraMovementDelta.x) * cameraSpeed;
                this.transform.localPosition = anchorPosition + currentPosition;
            }
        }
        else
        {
            this.transform.position = playerBoat.transform.position;
        }
    }
}
