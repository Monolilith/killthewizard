using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSlidePlayer : MonoBehaviour {

    [System.Serializable]
    public struct TextSlide
    {
        public string text;
        public float time;
    }

    

    [Header("Settings")]

    [SerializeField]
    TextSlide[] slides;
    [SerializeField]
    TextSlide[] tutorialSlides;
    [SerializeField]
    private string defaultText;
    [SerializeField]
    private string victoryText;
    [SerializeField]
    private string failureText;

    [Header("Links")]

    [SerializeField]
    private TMPro.TextMeshProUGUI textMesh;

    public static TextSlidePlayer Instance { get; private set; }

    private float slideTimer = 0f;
    private int slide = -1;
    private bool gameRunning = true;
    private bool fightMode = false;



    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!gameRunning)
            return;



        if(fightMode)
        {
            if (slide < slides.Length)
                slideTimer -= Time.deltaTime;

            if (slideTimer <= 0)
            {
                ++slide;
                if (slide >= slides.Length)
                    textMesh.text = defaultText;
                else
                {
                    slideTimer = slides[slide].time;
                    textMesh.text = slides[slide].text;
                }
            }
        }
        else
        {
            if (slide < tutorialSlides.Length)
                slideTimer -= Time.deltaTime;

            if (slideTimer <= 0)
            {
                ++slide;
                if (slide >= tutorialSlides.Length)
                    textMesh.text = defaultText;
                else
                {
                    slideTimer = tutorialSlides[slide].time;
                    textMesh.text = tutorialSlides[slide].text;
                }
            }
        }
    }

    public void EngageFightMode()
    {
        fightMode = true;
        slide = -1;
        slideTimer = 0f;
    }

    public void EndGame(bool playerWon)
    {
        gameRunning = false;

        if (playerWon)
            textMesh.text = victoryText;
        else
            textMesh.text = failureText;
    }

}
