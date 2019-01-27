using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour {

    [Header("Settings")]

    [SerializeField]
    private float fadeLength;

    [Header("Links")]

    [SerializeField]
    private UnityEngine.UI.Image blackImage;

    public static FadeToBlack Instance { get; private set; }

    private float timer;
    private bool fading;



    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(fading)
        {
            timer -= Time.deltaTime;
            if(timer < 0f)
                timer = 0f;

            Color color = blackImage.color;
            color.a = Mathf.InverseLerp(fadeLength, 0f, timer);
            blackImage.color = color;

            if (timer == 0f)
                fading = false;
        }
    }

    public void Fade()
    {
        fading = true;
        timer = fadeLength;
    }

}
