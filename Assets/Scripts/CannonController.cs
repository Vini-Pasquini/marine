using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public float BlastPower = 5;

    public GameObject CannonBall;
    public Transform ShotPoint;


    private void Update()
    {
        float HorizontalRotation = Input.GetAxis("Horizontal");
        float VerticalRotation = Input.GetAxis("Vertical");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
            new Vector3(0, HorizontalRotation * rotationSpeed, VerticalRotation * rotationSpeed));

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject CreateCannonBall = Instantiate(CannonBall, ShotPoint.position, ShotPoint.rotation);
            CreateCannonBall.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;

            //Screenshake.ShakeAmount = 2;
        }
    }
}
