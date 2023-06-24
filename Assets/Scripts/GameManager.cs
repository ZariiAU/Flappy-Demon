using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player player;
    [SerializeField] List<Pipe> pipes;
    [SerializeField] List<ParallaxBackground> parallaxBackgrounds;
    [SerializeField] List<GameObject> UIToDisableOnStart;
    [SerializeField] List<GameObject> UIToDisableOnEnd;
    [SerializeField] List<GameObject> UIToEnableOnEnd;
    [SerializeField] List<GameObject> UIToEnableOnStart;
    [SerializeField] List<GameObject> UIToEnableOnPause;
    [SerializeField] List<Button> BackButtons;

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

        foreach(Button button in BackButtons)
        {
            button.onClick.AddListener(() => {
                ToggleAllUI(true);
            });
        }

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject ui in UIToEnableOnPause)
                ui.SetActive(true);

            ToggleAllUI(false);
            
            
        }


        if (Input.GetKeyDown(KeyCode.Space) && hasLost) ReloadScene();

    }

    public void ToggleAllUI(bool on)
    {
        if (on)
        {
            if (gameStarted && !hasLost)
            {
                foreach (GameObject ui in UIToEnableOnStart)
                    ui.SetActive(true);
            }
            else if (hasLost)
            {
                foreach (GameObject ui in UIToEnableOnEnd)
                    EnableLossUI();
            }
            else
            {
                foreach (GameObject ui in UIToDisableOnStart)
                    ui.SetActive(true);

            }
            ToggleCursorVisibleLocked(false);
        }
        else {
            if (gameStarted && !hasLost)
            {
                foreach (GameObject ui in UIToEnableOnStart)
                    ui.SetActive(false);
            }
            else if (hasLost)
            {
                foreach (GameObject ui in UIToEnableOnEnd)
                    ui.SetActive(false);
            }
            else
            {
                foreach (GameObject ui in UIToDisableOnStart)
                    ui.SetActive(false);
            }
            ToggleCursorVisibleLocked(true);
        }
    }

    // TODO: Animate this somehow
    public void EnableLossUI()
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

    public IEnumerator DelayedTimeScaleZero()
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

    public void ToggleCursorVisibleLocked()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
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

    public void HideTitleUIInstant()
    {
        foreach (GameObject ui in UIToDisableOnStart)
            ui.SetActive(false);
        foreach (GameObject ui in UIToEnableOnStart)
            ui.SetActive(true);
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
