using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerBoat;

    private void Update()
    {
        this.transform.position = playerBoat.transform.position;
    }
}
