using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player player;
    public RockCutscene rockCut;
    public LoseCutscene loseCut;

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

    public void GameWin()
    {
        if (!timeIsRun) 
        { 
            return; 
        }
        timeIsRun = false;
        
        StopPlayer();

        rockCut.PlayWinCutscene();

        //Time.timeScale = 0f;
    }

    public void WinUI() 
    {
        winPanel.SetActive(true);
    }
    public void LoseUI()
    {
        losePanel.SetActive(true);
    }

    public void GameOver()
    {
        timeIsRun = false;
        StopPlayer();

        loseCut.PlayLoseCutscene();
    }

    void StopPlayer()
    { 
        if (player != null)
        {
            player.isStart = false;
            player.gameEnd = true;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            //player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

            print("bardeleate");
            if (player.chargeManager != null) player.chargeManager.gameObject.SetActive(false);
            if (player.catchRingObj != null) player.catchRingObj.SetActive(false);
            if (player.pushIconUI != null) player.pushIconUI.SetActive(false);
            if (player.missedIconUI != null) player.missedIconUI.SetActive(false);
        }
    }

}   
