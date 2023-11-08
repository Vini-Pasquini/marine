using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private float creditSpeed = 100f;
    private float cameraOffset = 900.1718f / 2;

    private void Update()
    {
        this.transform.localPosition = ScrollCredits();
    }

    private Vector3 ScrollCredits()
    {
        Vector3 newPosition = this.transform.localPosition;
        newPosition += Vector3.up * Time.deltaTime * creditSpeed;
        if (newPosition.y >= 3800 - cameraOffset) { newPosition.y = 3800 - cameraOffset; }
        return newPosition;
    }
}
