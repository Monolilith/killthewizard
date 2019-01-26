using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Settings")]

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpVel;
    [SerializeField]
    private float ungroundedThreshold;
    [SerializeField]
    private float groundCastSize;
    [SerializeField]
    private float groundCastDist;

    [Header("Links")]

    [SerializeField]
    private Rigidbody2D rb;

    private bool grounded;
    private static ContactPoint2D[] cp;
    private float prevVelY = 0f;



    private void Update()
    {

        float dt = Time.deltaTime;

        float velX = Input.GetAxis("Horizontal") * walkSpeed;

        Vector3 vel = rb.velocity;
        vel.x = velX;

        if (grounded && (Input.GetAxisRaw("Jump") > 0 || Input.GetAxisRaw("Vertical") > 0))
        {
            vel.y = jumpVel;
            grounded = false;
        }

        rb.velocity = vel;

    }

    private void FixedUpdate()
    {
        //Debug.Log(rb.velocity);
        //Debug.Log(grounded);

        float velY = rb.velocity.y;

        if (velY < -ungroundedThreshold || velY > ungroundedThreshold)
            grounded = false;

        if (Physics2D.BoxCast(transform.position, Vector2.one * groundCastSize, 0f, -Vector2.up, groundCastDist))
        {
            grounded = true;
        }

        prevVelY = velY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cp = collision.contacts;
        Debug.Log("COLLISION_NORMAL: " + collision.contacts[0].normal);

        Vector2 normal = collision.contacts[0].normal;

        if (normal.y > 0)
            grounded = true;
        else if(normal.x > 0 || normal.x < 0)
        {
            Vector2 vel = rb.velocity;
            vel.y = prevVelY;
            rb.velocity = vel;
        }
    }

}
