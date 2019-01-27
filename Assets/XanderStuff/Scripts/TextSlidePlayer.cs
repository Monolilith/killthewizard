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

    public enum EndState
    {
        VICTORY,
        FAILURE,
        TUTORIAL_COMPLETE
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
    [SerializeField]
    private string tutorialCompleteText;
    [SerializeField]
    private string doorEnteredText;

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
                {
                    EndGame(EndState.TUTORIAL_COMPLETE);
                }
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

    public void EndGame(EndState endState)
    {
        gameRunning = false;
        Wizard.Instance.EndGame();

        if (endState == EndState.VICTORY)
            textMesh.text = victoryText;
        else if (endState == EndState.FAILURE)
            textMesh.text = failureText;
        else if (endState == EndState.TUTORIAL_COMPLETE)
        {
            textMesh.text = tutorialCompleteText;
            Wizard.Instance.ActivateDoor();
        }
    }

    public void EnterDoor()
    {
        textMesh.text = doorEnteredText;
    }

}
