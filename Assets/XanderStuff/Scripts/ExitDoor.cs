using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if(pc != null)
        {
            audioSource.Play();
            Destroy(pc.gameObject);
            TextSlidePlayer.Instance.EnterDoor();
            FadeToBlack.Instance.Fade();
        }
    }

}
