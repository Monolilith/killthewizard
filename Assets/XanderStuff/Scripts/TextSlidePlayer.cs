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
    [SerializeField]
    private float charIdxAddWait;
    [SerializeField]
    private float defaultPitch;
    [SerializeField]
    private float minFightPitch;
    [SerializeField]
    private float maxFightPitch;

    [Header("Audio Links")]

    [SerializeField]
    private AudioClip tutorialSlideSound;
    [SerializeField]
    private AudioClip fightSlideSound;
    [SerializeField]
    private AudioClip victorySlideSound;
    [SerializeField]
    private AudioClip failureSlideSound;
    [SerializeField]
    private AudioClip tutorialCompleteSound;
    [SerializeField]
    private AudioClip doorEnteredSound;

    [Header("Links")]

    [SerializeField]
    private TMPro.TextMeshProUGUI textMesh;
    [SerializeField]
    private AudioSource audioSource;

    public static TextSlidePlayer Instance { get; private set; }

    private float slideTimer = 0f;
    private int slide = -1;
    private bool gameRunning = true;
    private bool fightMode = false;
    private int charidx = 0;
    private string slideText = "";
    private float charIdxAddTimer = 0f;



    private void Awake()
    {
        Instance = this;
        audioSource.pitch = defaultPitch;
    }

    private void Update()
    {
        if (charidx < slideText.Length)
        {
            charIdxAddTimer -= Time.deltaTime;
            if(charIdxAddTimer <= 0)
            {
                charIdxAddTimer = charIdxAddWait;
                ++charidx;
            }

            if (charidx == slideText.Length)
                audioSource.Stop();
        }

        textMesh.text = slideText.Substring(0, charidx);



        if (!gameRunning)
            return;



        if(fightMode)
        {
            if (slide < slides.Length)
                slideTimer -= Time.deltaTime;

            if (slideTimer <= 0)
            {
                ++slide;
                audioSource.pitch = Random.Range(minFightPitch, maxFightPitch);
                audioSource.PlayOneShot(fightSlideSound);
                if (slide >= slides.Length)
                {
                    //textMesh.text = defaultText;
                    SetSlideText(defaultText);
                    slideTimer = float.MaxValue;
                }
                else
                {
                    slideTimer = slides[slide].time;
                    //textMesh.text = slides[slide].text;
                    SetSlideText(slides[slide].text);
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
                audioSource.PlayOneShot(tutorialSlideSound);
                if (slide >= tutorialSlides.Length)
                {
                    EndGame(EndState.TUTORIAL_COMPLETE);
                }
                else
                {
                    slideTimer = tutorialSlides[slide].time;
                    //textMesh.text = tutorialSlides[slide].text;
                    SetSlideText(tutorialSlides[slide].text);
                }
            }
        }
    }

    public void EngageFightMode()
    {
        fightMode = true;
        gameRunning = true;
        slide = -1;
        slideTimer = 0f;
    }

    public void EndGame(EndState endState)
    {
        if (!gameRunning)
            return;

        audioSource.pitch = defaultPitch;

        gameRunning = false;
        Wizard.Instance.EndGame();

        if (endState == EndState.VICTORY)
        {
            if (victorySlideSound)
                audioSource.PlayOneShot(victorySlideSound);
            //textMesh.text = victoryText;
            SetSlideText(victoryText);
        }
        else if (endState == EndState.FAILURE)
        {
            if (failureSlideSound)
                audioSource.PlayOneShot(failureSlideSound);
            //textMesh.text = failureText;
            SetSlideText(failureText);
            FadeToBlack.Instance.Fade();
        }
        else if (endState == EndState.TUTORIAL_COMPLETE)
        {
            if (tutorialSlideSound)
                audioSource.PlayOneShot(tutorialSlideSound);
            //textMesh.text = tutorialCompleteText;
            SetSlideText(tutorialCompleteText);
            Wizard.Instance.ActivateDoor();
        }
    }

    public void EnterDoor()
    {
        if (doorEnteredSound)
            audioSource.PlayOneShot(doorEnteredSound);
        //textMesh.text = doorEnteredText;
        SetSlideText(doorEnteredText);
    }

    private void SetSlideText(string s)
    {
        slideText = s;
        charidx = 0;
    }

}
