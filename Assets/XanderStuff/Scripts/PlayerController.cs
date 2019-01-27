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
    [SerializeField]
    private float swipeProjectileSpeed;
    [SerializeField]
    private float swipeLifetime;
    [SerializeField]
    private float swipeCooldown;

    [Header("Physics Settings")]

    [SerializeField]
    private float fallAccelMultiplier;
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
    [SerializeField]
    private GameObject swipePrefab;

    public int hp { get; private set; }
    public int MaxHealth { get { return maxHealth; } }
    public static PlayerController Instance { get; private set; }

    private bool grounded;
    private bool rightWalled;
    private bool leftWalled;
    private static ContactPoint2D[] cp;
    private float swipeHeat;

    //private float prevPosX;




    private void Awake()
    {
        hp = maxHealth;
        Instance = this;
    }

    private void Update()
    {

        float dt = Time.deltaTime;
        float fdt = Time.fixedDeltaTime;

        float velX = Input.GetAxisRaw("Horizontal") * walkSpeed;

        Vector3 vel = rb.velocity;
        
        if((velX > 0 && !rightWalled) || (velX < 0 && !leftWalled) || velX == 0f)
            vel.x = velX;

        if (grounded && (Input.GetAxisRaw("Jump") > 0 || Input.GetAxisRaw("Vertical") > 0))
        {
            vel.y = jumpVel;
            grounded = false;
        }

        rb.velocity = vel;



        if (swipeHeat > 0)
            swipeHeat -= dt;

        if(Input.GetMouseButtonDown(0) && swipeHeat <= 0)
        {
            swipeHeat = swipeCooldown;

            GameObject obj = Instantiate(swipePrefab);
            obj.transform.position = transform.position;

            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

            obj.GetComponent<Rigidbody2D>().velocity = obj.transform.up * swipeProjectileSpeed;
            Destroy(obj, swipeLifetime);
        }

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

        if(!grounded && velY < 0)
        {
            velY += Physics.gravity.y * Time.fixedDeltaTime * fallAccelMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, velY);
        }
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
        UIManager.Instance.DoHurt();
        if (hp == 0)
            Kill();
    }

    private void Kill()
    {
        Wizard.Instance.EndGame();
        TextSlidePlayer.Instance.EndGame(TextSlidePlayer.EndState.FAILURE);
    }

}
