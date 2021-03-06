﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour {

    [System.Serializable]
    public struct BulletWave
    {
        public float waitBeforeLaunch;
        public int bulletCount;
        public Sprite sprite;
        public bool wordWave;
        public Sprite[] wordSprites;
    }




    [Header("Settings")]

    [SerializeField]
    private BulletWave[] waves;
    [SerializeField]
    private float bulletLifetime;
    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float riseToFightTime;

    [Header("Audio Links")]

    [SerializeField]
    private AudioClip shootSound;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private AudioClip mainMusic;
    [SerializeField]
    private AudioClip fightMusic;
    [SerializeField]
    private AudioClip failureFanfare;

    [Header("Links")]

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform fightPos;
    [SerializeField]
    private GameObject exitDoor;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource musicPlayer;

    public static Wizard Instance { get; private set; }
    public float MaxHealth { get { return maxHealth; } }
    public float hp { get; private set; }
    public bool gameCompleted { get; private set; }

    private int wave = -1;
    private float timer;
    private bool fightMode = false;


    Animator animator;

    private float riseTimer;



    private void Awake()
    {
        Instance = this;
        hp = maxHealth;
    }

    private void Start()
    {
        transform.position = startPos.position;
        exitDoor.SetActive(false);

        animator = GetComponent<Animator>();
        animator.SetBool("GetHit", false);
    }

    private void Update()
    {
        if (gameCompleted)
            return;

        float dt = Time.deltaTime;

        if(fightMode)
        {
            animator.SetBool("GetHit", true);
            if (riseTimer <= riseToFightTime)
            {
                riseTimer += dt;
                if (riseTimer > riseToFightTime)
                    riseTimer = riseToFightTime;

                float xpos = transform.position.x;
                float ypos = Mathf.Lerp(startPos.position.y, fightPos.position.y, Mathf.InverseLerp(0f, riseToFightTime, riseTimer));
                transform.position = new Vector3(xpos, ypos);
            }

            if (wave < waves.Length)
                timer -= dt;

            if (timer <= 0)
            {
                ++wave;
                if (wave < waves.Length)
                {
                    timer = waves[wave].waitBeforeLaunch;
                    SpawnWave();
                }
            }
        }
    }

    private void SpawnWave()
    {
        audioSource.PlayOneShot(shootSound);

        float increment = -180f / (waves[wave].bulletCount + 1);

        for(int i = 1; i <= waves[wave].bulletCount; ++i)
        {
            GameObject obj = Instantiate(bulletPrefab);
            Transform t = obj.transform;
            SpriteRenderer r = obj.GetComponent<SpriteRenderer>();
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            BoxCollider2D c = obj.GetComponent<BoxCollider2D>();

            if(!waves[wave].wordWave)
                r.sprite = waves[wave].sprite;
            else
                r.sprite = waves[wave].wordSprites[waves[wave].bulletCount - i];

            Vector2 S = r.sprite.bounds.size;
            c.size = S;
            //c.offset = new Vector2((S.x / 2), 0);

            t.position = transform.position;
            t.eulerAngles = new Vector3(0f, 0f, increment * i);

            rb.velocity = t.right * bulletVelocity;

            Destroy(obj, bulletLifetime);
        }
    }

    public void EndGame()
    {
        gameCompleted = true;
        musicPlayer.clip = mainMusic;
        musicPlayer.Stop();
        musicPlayer.Play();
        if(PlayerController.Instance.hp <= 0)
            audioSource.PlayOneShot(failureFanfare);
    }

    public void ActivateDoor()
    {
        exitDoor.SetActive(true);
        audioSource.pitch = 1f;
    }

    public void Damage(float amt)
    {
        hp -= amt;

        if(hitSound)
            audioSource.PlayOneShot(hitSound);

        if(!fightMode)
        {
            musicPlayer.clip = fightMusic;
            musicPlayer.Stop();
            musicPlayer.Play();
            fightMode = true;
            timer = waves[0].waitBeforeLaunch;
            BlackPanelRise.Instance.Rise();
            TextSlidePlayer.Instance.EngageFightMode();
            exitDoor.SetActive(false);
        }

        if(hp <= 0)
        {
            hp = 0;
            TextSlidePlayer.Instance.EndGame(TextSlidePlayer.EndState.VICTORY);
            //EndGame();
            UIManager.Instance.Poof();
            gameCompleted = true;
        }
    }

}
