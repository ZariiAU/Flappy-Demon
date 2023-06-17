using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Player player;
    [SerializeField] List<Pipe> pipes;
    [SerializeField] List<ParallaxBackground> parallaxBackgrounds;
    [SerializeField] List<GameObject> UIToDisableOnStart;
    [SerializeField] List<GameObject> UIToDisableOnEnd;
    [SerializeField] List<GameObject> UIToEnableOnEnd;
    [SerializeField] List<GameObject> UIToEnableOnStart;
    public UnityEvent GameStarted;

    public GameObject quitButton;
    public bool hasLost = false;
    public bool gameStarted = false;

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
        Time.timeScale = 1;
        player = FindObjectOfType<Player>();

        ToggleCursorVisibleLocked(false);

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
                foreach (Collider2D c in pipe.GetComponentsInChildren<Collider2D>())
                {
                    c.enabled = false;
                }
            }
        });

        if(Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            quitButton.SetActive(false);
        }
        else
            quitButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Application.platform == RuntimePlatform.WindowsPlayer)
        {
            QuitApplication();
        }

        if (hasLost)
        {
            EnableLossUI();
            player.disableInputs = true;
            StartCoroutine(DelayedTimeScaleZero());
        }
    }

    // TODO: Animate this somehow
    void EnableLossUI()
    {
        foreach (GameObject ui in UIToEnableOnEnd)
        {
            ui.SetActive(true);
            foreach (ScoreTextUI UI in ui.GetComponentsInChildren<ScoreTextUI>())
            {
                if (UI.scoreType == ScoreUIType.CurrentScore && UI.gameObject.activeSelf)
                    UI.ChangeText(ScoreKeeper.Instance.Score);
                else if (UI.scoreType == ScoreUIType.Highscore)
                    UI.ChangeText(ScoreKeeper.Instance.HighScore);
            }
        }
        foreach (GameObject ui in UIToDisableOnEnd)
        {
            ui.SetActive(false);
        }
        ToggleCursorVisibleLocked(true);
    }

    IEnumerator DelayedTimeScaleZero()
    {
        yield return new WaitForSeconds(4);
        Time.timeScale = 0;
    }

    public void ReloadScene()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Quits application
    public void QuitApplication()
    {
        Application.Quit();
    }

    // Toggles Cursor Visibility & Lock mode
    public void ToggleCursorVisibleLocked(bool visible)
    {
        if(visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked ;
            Cursor.visible = false;
        }
    }

    public void StartGame()
    {
        GameStarted.Invoke();
        gameStarted = true;
        foreach(GameObject ui in UIToDisableOnStart)
        {
            if (ui.GetComponentInChildren<Mover>())
            {
                ui.GetComponentInChildren<Mover>().shouldMove = true;
            }
        }
        StartCoroutine(HideTitleUI());
    }

    IEnumerator HideTitleUI()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject ui in UIToDisableOnStart)
            ui.SetActive(false);
        foreach (GameObject ui in UIToEnableOnStart)
            ui.SetActive(true);
    }
}
