using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if(pc != null)
        {
            Destroy(pc.gameObject);
            TextSlidePlayer.Instance.EnterDoor();
        }
    }

}
