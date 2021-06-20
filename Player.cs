using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed; //5
    public VirtualJoystick joystick;
    private Rigidbody2D rg;
    private Animator anim;

    public static float horizontal;
    public static float vertical;

    public GameObject bullet;
    public int shootSpeed; //5
    private Rigidbody2D bulletRb;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = joystick.Horizontal();
        vertical = joystick.Vertical();
        rg.velocity = new Vector2(joystick.Horizontal(), joystick.Vertical()) * speed;

        anim.SetFloat("Horizontal", joystick.Horizontal());
        anim.SetFloat("Vertical", joystick.Vertical());
        anim.SetFloat("Magnitude", new Vector2(joystick.Horizontal(), joystick.Vertical()).magnitude);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Melee();
        }
    }

    public void Shoot()
    {
        if(joystick.Horizontal() != 0 || joystick.Vertical() != 0)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.AngleAxis(Mathf.Atan2(joystick.Horizontal(), joystick.Vertical()) * Mathf.Rad2Deg, Vector3.back));
            bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRb.velocity = new Vector2(joystick.Horizontal(), joystick.Vertical()).normalized * shootSpeed;
        }
        
        if (joystick.Horizontal() == 0 && joystick.Vertical() == 0)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.AngleAxis(Mathf.Atan2(0, -1) * Mathf.Rad2Deg, Vector3.back));
            bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRb.velocity = new Vector2(0, -1)* shootSpeed;
        }
    }

    public void Melee()
    {

    }
}