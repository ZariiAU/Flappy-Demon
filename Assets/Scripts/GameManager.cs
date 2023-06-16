using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;
    [SerializeField] List<Pipe> pipes;
    public GameObject gameLostUI;
    public bool hasLost = false;

    public static GameManager Instance { get; private set; }


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
        player = FindObjectOfType<Player>();

        Cursor.lockState = CursorLockMode.Locked;

        player.playerDied.AddListener(() =>
        {
            hasLost = true;
            
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLost)
        {
            EnableLossUI();
            player.disableInputs = true;
        }
    }

    void EnableLossUI()
    {
        gameLostUI.SetActive(true);
        foreach (ScoreTextUI UI in gameLostUI.GetComponentsInChildren<ScoreTextUI>())
        {
            if (UI.scoreType == ScoreUIType.CurrentScore)
                UI.ChangeText(ScoreKeeper.Instance.Score.ToString());
            else if (UI.scoreType == ScoreUIType.Highscore)
                UI.ChangeText(ScoreKeeper.Instance.HighScore.ToString());
        }
    }
}
