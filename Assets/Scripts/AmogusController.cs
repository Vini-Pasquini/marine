using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmogusController : MonoBehaviour
{
    private float timer = 3;

    private void Update()
    {
        Vector3 currentRotation = this.transform.rotation.eulerAngles;
        currentRotation += (Vector3.up * Time.deltaTime * 90f);
        this.transform.rotation = Quaternion.Euler(currentRotation);

        if (Input.GetKey(KeyCode.H)) timer -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.H)) timer = 3f;
        if (timer <= 0f) this.GetComponent<MeshRenderer>().enabled = true;
    }
}
