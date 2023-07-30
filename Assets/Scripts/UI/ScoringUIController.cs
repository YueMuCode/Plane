using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoringUIController : MonoBehaviour
{
    [Header("---Background---")] 
    [SerializeField] private Image background;
    [SerializeField] private Sprite[] backgroundImages;

    [Header("---ScoringScreen---")] 
    [SerializeField] private Canvas scoringScreenCanvas;

    [SerializeField] private Text playerScoreText;
    [SerializeField] private Button buttonMainMenu;
    [SerializeField] private Transform highScoreLeaderboardContainer;

    [Header("---High Score Screen---")] 
    [SerializeField]private Canvas newHighScoreScreenCanvas;
    [SerializeField]private Button buttonCancel;
    [SerializeField]private Button buttonSubmit;
    [SerializeField]private InputField playerNameInputField;
    private void Start()
    { 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();
        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScoringScreen(); 
        }
        
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonMainMenu.gameObject.name,OnButtonMainMenuClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonSubmit.gameObject.name,OnButtonSubmitClicked);
        ButtonPressedBehaviour.buttonFunctionTable.Add(buttonCancel.gameObject.name,HideNewHighScoreScreen);
        GameManager.GameState = GameState.Scoring;
    }

    private void ShowNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonCancel);
    }
    private void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }
    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctionTable.Clear();
    }

    void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
    }

    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        
        //更新高分排行榜\
        UpdateHighScoreLeaderboard();
    }

    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.loadPlayerScoreData().List;
        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            var child = highScoreLeaderboardContainer.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }
    
    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }

        HideNewHighScoreScreen();
    }

    
}
