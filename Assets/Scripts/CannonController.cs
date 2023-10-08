using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float rotationSpeed = 0.6f;
    public float BlastPower = 0;

    public GameObject CannonBall;
    public Transform ShotPoint;

    public float timer = 0.0f;

    public bool canShoot = true;


    public float yRotation;
    public float ZRotation;

    private void Update()
    {
        BlastPower = GameObject.FindGameObjectWithTag("Alavanca").GetComponent<GearManager>().GetBulletSpeed();

        MoveHarpoon();
        Shoot();
    }

    private void MoveHarpoon()
    {
        yRotation = gameObject.transform.rotation.y;

        float HorizontalRotation = Input.GetAxis("Horizontal");
        float VerticalRotation = Input.GetAxis("Vertical");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
            new Vector3(0, HorizontalRotation * rotationSpeed, VerticalRotation * rotationSpeed));

        if(yRotation >= 180.0f)
        {
            Debug.Log("Passou");
            yRotation = 180.0f;
        }

    }

    private void Shoot()
    {
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject CreateCannonBall = Instantiate(CannonBall, ShotPoint.position, ShotPoint.rotation);
                CreateCannonBall.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;
                Destroy(CreateCannonBall, 5);
                canShoot = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= 2.5f)
            {
                timer = 0;
                canShoot = true;
            }
        }
    }
}
