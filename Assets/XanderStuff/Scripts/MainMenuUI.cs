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



    public void GoToGamesPanel()
    {
        startPanel.SetActive(false);
        gamesPanel.SetActive(true);
    }

    public void TryStartGame()
    {
        if(nameField.text != "")
        {
            SceneBridge.Instance.characterName = nameField.text;
            SceneManager.LoadSceneAsync(gameSceneName);
        }
    }

}
