using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private int score;
    private int highScore;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;

    public static ScoreKeeper Instance { get; private set; }

    public int Score { get { return score; } set { score = value; scoreText.text = score.ToString(); } }
    public int HighScore { 
        get { return highScore; } 
        set { 
            highScore = value; 
            PlayerPrefs.SetInt("Highscore", HighScore);
            highscoreText.text = HighScore.ToString();
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
