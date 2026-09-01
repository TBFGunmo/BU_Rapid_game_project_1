using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player player;
    public RockCutscene rockCut;

    public float timeGame = 60f;
    private float timeRemain;
    private bool timeIsRun = false;
    private bool gameHasStarted = false;

    public TMP_Text timeText;
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timeRemain = timeGame;
        timeText.text = "";
    }

    void Update()
    {
        if (timeIsRun)
        {
            if (timeRemain > 0)
            {
                timeRemain -= Time.deltaTime;
                UpdateTimerUI();
            }
            else 
            {
                timeRemain = 0;
                timeIsRun   = false;
                UpdateTimerUI();
                GameOver();
            }
        }
    }

    public void StartGame()
    {
        if (gameHasStarted)
        {
            return;
        }

        gameHasStarted = true;
        timeIsRun = true;

        if (player != null)
        {
            player.StartGame();
        }
    }

    void UpdateTimerUI()
    {
      
        int minutes = Mathf.FloorToInt(timeRemain / 60);
        int seconds = Mathf.FloorToInt(timeRemain % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemain <= 10f && timeRemain > 0f)
        {
            float blink = Mathf.PingPong(Time.time * 5f, 1f);
            timeText.color = Color.Lerp(Color.red, Color.white, blink);
        }
        else
        {
            timeText.color = Color.white;
        }
    }

    public void GameOver()
    {
        timeIsRun = false;
        losePanel.SetActive(true);
        StopPlayer();
        Time.timeScale = 0f;
    }

    public void GameWin()
    {
        if (!timeIsRun) 
        { 
            return; 
        }
        timeIsRun = false;
        winPanel.SetActive(true);
        StopPlayer();

        rockCut.PlayWinCutscene();

        Time.timeScale = 0f;
    }

    void StopPlayer()
    { 
        if (player != null)
        {
            player.isStart = false;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

}
