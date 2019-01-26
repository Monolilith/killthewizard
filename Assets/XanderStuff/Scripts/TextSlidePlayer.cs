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
    private string defaultText;
    [SerializeField]
    private string victoryText;
    [SerializeField]
    private string failureText;

    [Header("Links")]

    [SerializeField]
    private TMPro.TextMeshProUGUI textMesh;

    private float slideTimer = 0f;
    private int slide = -1;
    private bool gameRunning = true;



    private void Update()
    {
        if (!gameRunning)
            return;



        if(slide < slides.Length)
            slideTimer -= Time.deltaTime;

        if(slideTimer <= 0)
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

    public void EndGame(bool playerWon)
    {
        gameRunning = false;

        if (playerWon)
            textMesh.text = victoryText;
        else
            textMesh.text = failureText;
    }

}
