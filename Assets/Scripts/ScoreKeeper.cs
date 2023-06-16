using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private int score;
    private int highScore;
    public UnityEvent<int> scoreUpdated;
    public UnityEvent<int> highscoreUpdated;
    public static ScoreKeeper Instance { get; private set; }

    public int Score { get { return score; } set { score = value; scoreUpdated.Invoke(Score); } }
    public int HighScore { 
        get { return highScore; } 
        set { 
            highScore = value; 
            PlayerPrefs.SetInt("Highscore", HighScore);
            highscoreUpdated.Invoke(HighScore);
        } 
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        HighScore = PlayerPrefs.GetInt("Highscore");
    }
}
