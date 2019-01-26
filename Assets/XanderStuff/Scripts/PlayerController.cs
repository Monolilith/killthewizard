using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Settings")]

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float ungroundedThreshold;

    [Header("Links")]

    [SerializeField]
    private Rigidbody2D rb;

    private bool grounded;
    private static ContactPoint2D[] cp;



    private void Update()
    {

        float dt = Time.deltaTime;

        float velX = Input.GetAxis("Horizontal") * walkSpeed * dt;

        Vector3 vel = rb.velocity;
        vel.x = velX;
        rb.velocity = vel;

        rb.AddForce(Vector2.up * Input.GetAxis("Jump") * jumpForce);

    }

    private void FixedUpdate()
    {
        //Debug.Log(rb.velocity);
        //Debug.Log(grounded);

        float velY = rb.velocity.y;

        if (velY < -ungroundedThreshold || velY > ungroundedThreshold)
            grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cp = collision.contacts;
        Debug.Log("COLLISION_NORMAL: " + collision.contacts[0].normal);

        if (collision.contacts[0].normal.y > 0)
            grounded = true;
    }

}
