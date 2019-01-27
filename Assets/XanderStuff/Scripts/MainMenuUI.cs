using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    [SerializeField]
    private string gameSceneName;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject gamesPanel;
    [SerializeField]
    private TMPro.TMP_InputField nameField;
    [SerializeField]
    private AudioSource audioSource;

    [Header("Audio Links")]

    [SerializeField]
    private AudioClip buttonClickSound;
    [SerializeField]
    private AudioClip startGameSound;
    [SerializeField]
    private AudioClip failStartGameSound;

    private bool waitingToStart = false;



    private void Update()
    {
        if(waitingToStart && !audioSource.isPlaying)
            SceneManager.LoadSceneAsync(gameSceneName);
    }

    public void GoToGamesPanel()
    {
        startPanel.SetActive(false);
        gamesPanel.SetActive(true);
    }

    public void TryStartGame()
    {
        if(nameField.text != "")
        {
            waitingToStart = true;
            audioSource.clip = startGameSound;
            audioSource.Play();
            SceneBridge.Instance.characterName = nameField.text;
        }
        else
        {
            if(failStartGameSound)
                audioSource.PlayOneShot(failStartGameSound);
        }
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

}
