using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if(p != null)
        {
            if(Wizard.Instance.hp > 0)
                p.Damage();
            Destroy(gameObject);
        }
    }

}
