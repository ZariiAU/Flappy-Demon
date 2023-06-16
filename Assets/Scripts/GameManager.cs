using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;
    [SerializeField] List<Pipe> pipes;
    [SerializeField] List<ParallaxBackground> parallaxBackgrounds;
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
        Cursor.visible = false;

        player.playerDied.AddListener(() =>
        {
            hasLost = true;

            // Stop background
            foreach (ParallaxBackground parallaxBackground in parallaxBackgrounds)
            {
                parallaxBackground.xSpeed = 0;
                parallaxBackground.ySpeed = 0;
            }

            // Stop Pipes
            foreach(Pipe pipe in pipes)
            {
                pipe.xSpeed = 0;
                pipe.ySpeed = 0;
                foreach (Collider c in pipe.GetComponentsInChildren<Collider>())
                {
                    c.enabled = false;
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLost)
        {
            EnableLossUI();
            player.disableInputs = true;
            StartCoroutine(DelayedTimeScaleZero());
        }
    }

    void EnableLossUI()
    {
        gameLostUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (ScoreTextUI UI in gameLostUI.GetComponentsInChildren<ScoreTextUI>())
        {
            if (UI.scoreType == ScoreUIType.CurrentScore)
                UI.ChangeText(ScoreKeeper.Instance.Score.ToString());
            else if (UI.scoreType == ScoreUIType.Highscore)
                UI.ChangeText(ScoreKeeper.Instance.HighScore.ToString());
        }
    }

    IEnumerator DelayedTimeScaleZero()
    {
        yield return new WaitForSeconds(4);
        Time.timeScale = 0;
    }
}
