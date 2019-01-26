using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Gameplay Settings")]

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float jumpVel;
    [SerializeField]
    private int maxHealth;

    [Header("Physics Settings")]

    [SerializeField]
    private float ungroundedThreshold;
    [SerializeField]
    private float groundCastSize;
    [SerializeField]
    private float groundCastDist;
    [SerializeField]
    private float wallCastSize;
    [SerializeField]
    private float wallCastDist;
    [SerializeField]
    private LayerMask playerMask;

    [Header("Links")]

    [SerializeField]
    private Rigidbody2D rb;

    public int hp { get; private set; }
    public int MaxHealth { get { return maxHealth; } }
    public static PlayerController Instance { get; private set; }

    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;
    private static ContactPoint2D[] cp;
    private float prevVelY = 0f;



    private void Awake()
    {
        hp = maxHealth;
        Instance = this;
    }

    private void Update()
    {

        float dt = Time.deltaTime;
        float fdt = Time.fixedDeltaTime;

        float velX = Input.GetAxis("Horizontal") * walkSpeed;

        Vector3 vel = rb.velocity;
        
        if((velX > 0 && !rightWalled) || (velX < 0 && !leftWalled))
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

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * groundCastSize, 0f, -Vector2.up, groundCastDist, playerMask);
        
        if (hit.transform != null)
            grounded = true;

        hit = Physics2D.BoxCast(transform.position, Vector2.one * wallCastSize, 0f, Vector2.right, wallCastDist, playerMask);
        if (hit.transform != null)
            rightWalled = true;
        else
            rightWalled = false;

        hit = Physics2D.BoxCast(transform.position, Vector2.one * wallCastSize, 0f, -Vector2.right, wallCastDist, playerMask);
        if (hit.transform != null)
            leftWalled = true;
        else
            leftWalled = false;

        prevVelY = velY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cp = collision.contacts;
        Debug.Log("COLLISION_NORMAL: " + collision.contacts[0].normal);

        Vector2 normal = collision.contacts[0].normal;

        if (normal.y > 0)
            grounded = true;
    }

    public void Damage()
    {
        --hp;
        if (hp == 0)
            Kill();
    }

    private void Kill()
    {
        Wizard.Instance.EndGame();
        TextSlidePlayer.Instance.EndGame(false);
    }

}
