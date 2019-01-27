using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwipe : MonoBehaviour
{

    [SerializeField]
    private float damage;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Wizard w = collision.gameObject.GetComponent<Wizard>();
        if (w != null)
        {
            w.Damage(damage);
            Destroy(gameObject);

        }



    }
}
