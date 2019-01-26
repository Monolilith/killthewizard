using System.Collections;
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

    [Header("Links")]

    [SerializeField]
    private GameObject bulletPrefab;

    private int wave = -1;
    private float timer;
    private bool gameRunning = true;



    private void Update()
    {
        if (!gameRunning)
            return;



        if (wave < waves.Length)
            timer -= Time.deltaTime;

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

    private void SpawnWave()
    {
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
            c.offset = new Vector2((S.x / 2), 0);

            t.position = transform.position;
            t.eulerAngles = new Vector3(0f, 0f, increment * i);

            rb.velocity = t.right * bulletVelocity;

            Destroy(obj, bulletLifetime);
        }
    }

    public void EndGame()
    {
        gameRunning = false;
    }

}
