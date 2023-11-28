using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    public float rotationSpeed = 0.6f;
    public float BlastPower = 22;

    public GameObject CannonBall;
    public Transform ShotPoint;

    public float timer = 2.5f;

    public bool canShoot = true;
    public bool hasAmmo = true;

    public int bulletsCounts = 15;

    [SerializeField] private TextMeshProUGUI bulletDisplay;

    public float yRotation;
    public float ZRotation;

    private Animator harpoonAnim;
    private bool reloadAnim = true;

    private void Start()
    {
        bulletDisplay.text = "AMMO:\n" + bulletsCounts.ToString();
        harpoonAnim = this.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        BlastPower = GameObject.FindGameObjectWithTag("Alavanca").GetComponent<Slider>().value;
        //BlastPower = GameObject.FindGameObjectWithTag("Alavanca").GetComponent<GearManager>().GetBulletSpeed();

        if (Input.GetKeyDown(KeyCode.PageDown)) { bulletsCounts -= 5; bulletDisplay.text = "AMMO:\n" + bulletsCounts.ToString(); } // DEBUG< TIRAR DPS

        if (bulletsCounts <= 0)
        {
            hasAmmo = false;
        }

        MoveHarpoon();
        Shoot();
    }

    private void MoveHarpoon()
    {
        yRotation = gameObject.transform.rotation.y;

        float HorizontalRotation = Input.GetAxis("Horizontal");
        float VerticalRotation = Input.GetAxis("Vertical");

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
            new Vector3(-VerticalRotation * rotationSpeed, HorizontalRotation * rotationSpeed, 0));

        if(yRotation >= 180.0f)
        {
            Debug.Log("Passou");
            yRotation = 180.0f;
        }

    }

    private void Shoot()
    {
        if (canShoot && hasAmmo)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                harpoonAnim.Play("Shoot");
                bulletsCounts -= 1;
                bulletDisplay.text = "AMMO:\n" + bulletsCounts.ToString();
                GameObject CreateCannonBall = Instantiate(CannonBall, ShotPoint.position, ShotPoint.rotation);
                CreateCannonBall.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;
                Destroy(CreateCannonBall, 5);
                timer = 2.5f;
                reloadAnim = true;
                canShoot = false;
            }
        }

        timer -= Time.deltaTime;

        if (timer <= .5f && reloadAnim)
        {
            harpoonAnim.Play("Reload");
            reloadAnim = false;
        }

        if (timer <= 0 && !canShoot)
        {
            canShoot = true;
        }
    }
}
