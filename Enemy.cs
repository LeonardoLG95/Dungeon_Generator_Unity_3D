using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed; //3
    private GameObject target;
    private int actualPoint = 0;
    public static int numberRoom=0;

    bool up = false;
    bool down = false;
    bool right = false;
    bool left = false;
    bool rightleftDecition = false;
    bool updownDecition = false;

    bool targetTargeted = false;

    Vector2 rightleftside = new Vector2();
    Vector2 updownside = new Vector2();

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(up || down)
        {
            if (!rightleftDecition)
            {
                int rdn = Random.Range(0, 2);
                if (rdn == 0) rightleftside = Vector2.right;
                if (rdn == 1) rightleftside = Vector2.left;
                rightleftDecition = true;
            }
            transform.Translate(rightleftside * speed * Time.deltaTime);
        }

        if (right || left)
        {
            if (!updownDecition)
            {
                int rdn = Random.Range(0, 2);
                if (rdn == 0) updownside = Vector2.up;
                if (rdn == 1) updownside = Vector2.down;
                updownDecition = true;
            }
            transform.Translate(updownside * speed * Time.deltaTime);
        }

        if ((right && up) || (right && down))
        {
            rightleftside = Vector2.left;
            transform.Translate(rightleftside * speed * Time.deltaTime);
        }

        if ((left && up) || (left && down))
        {
            rightleftside = Vector2.right;
            transform.Translate(rightleftside * speed * Time.deltaTime);
        }

        

        if (!up && !down && !right && !left && !targetTargeted) transform.position = transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!up && !down && !right && !left)
            {
                targetTargeted = true;
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
                if ((target.transform.position - transform.position).magnitude <= 1) transform.position = transform.position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            targetTargeted = false;
            transform.position = transform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Wall")
        {
            float angle = Vector3.Angle(collision.contacts[0].normal, Vector2.down);

            if (Mathf.Approximately(angle, 0)) up = true;// back
            if (Mathf.Approximately(angle, 180)) down = true;// front
            if (Mathf.Approximately(angle, 90))
            {
                if (collision.transform.position.x > transform.position.x) right = true;

                if (collision.transform.position.x < transform.position.x) left = true;
            }
        }
        

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        up = false;
        down = false;
        right = false;
        left = false;

        rightleftDecition = false;
        updownDecition = false;
    }
}


