using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {
    
    private Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 delta = new Vector3();

        if (Input.GetKey(KeyCode.D))
            delta.x = speed;

        if (Input.GetKey(KeyCode.A))
            delta.x = -speed;

        transform.position += delta;

        if(Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            rb.AddForce(transform.up * jumpHeight);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
            isJumping = false;
    }

}
