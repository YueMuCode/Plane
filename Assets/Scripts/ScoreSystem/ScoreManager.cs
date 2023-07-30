using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
 {
     #region DIsPlay

 
     public int Score => score;
    private int score;
    private int currentScore;
    private Vector3 scoreTextScale = new Vector3(2f, 2.7f, 1f);
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateScoreText(score);
    }
 
    public void AddScore(int scorePoint)
    {
        currentScore+= scorePoint;
        StartCoroutine(AddScoreCoroutine());
    }

    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (score<currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateScoreText(score);
            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);
    }
    #endregion
    
     #region High ScoreSystem

    [System.Serializable] public class PlayerScore
     {
         public int score;
         public string playerName;

         public PlayerScore(int score, string playerName)
         {
             this.score = score;
             this.playerName = playerName;
         }
     }
    [System.Serializable] public class PlayerScoreData
    {
        public List<PlayerScore> List = new List<PlayerScore>();
    }

    private readonly string SaveFileName = "player_score.json";
    private string playerName = "No Name";
    public bool HasNewHighScore => score > loadPlayerScoreData().List[9].score;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }
    
    
    public void SavePlayerScoreData()
    {
        var playerScoreData = loadPlayerScoreData();
        playerScoreData.List.Add(new PlayerScore(score,playerName));
        playerScoreData.List.Sort((x,y)=>y.score.CompareTo(x.score));//Ωµ–Ú≈≈–Ú
        SaveSystem.Save(SaveFileName,playerScoreData);
    }
    
    
    public PlayerScoreData loadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();
        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.List.Count < 10)
            {
                playerScoreData.List.Add(new PlayerScore(0,playerName));
            }
            SaveSystem.Save(SaveFileName,playerScoreData);
        }

        return playerScoreData;
    }
     #endregion
}
