using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private float creditSpeed = 10f;

    private void Update()
    {
        this.transform.position += Vector3.up * Time.deltaTime * creditSpeed;
    }
}
