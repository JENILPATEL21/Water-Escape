using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class uiManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public Image[] heartIcons;
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;
    public GameObject ball;
    public GameObject enemySpawner;
    public GameObject coinSpawner;
    private int score = 0;
    public int playerLives = 3;
    public float gameTime = 60f;
    public bool gameEnded = false;
    private GameOverManager gameOverManager;
    public Image countdownImage;
    public Sprite countdown3Sprite;
    public Sprite countdown2Sprite;
    public Sprite countdown1Sprite;
    public Sprite goSprite;
    public TrackLooper trackmovement;

    public GameObject finishPanel;
    public float finishPanelDelay = 2f; // Time before showing the win panel

    public int gameplayed = 1;

    public float scoreIncreaseRate = 1f;

    void Start()
    {
        score = 0;
        playerLives = heartIcons.Length;
        gameOverManager = FindObjectOfType<GameOverManager>();

        if(countdownPanel != null) countdownPanel.SetActive(true);
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (ball != null) ball.SetActive(false);
        if (enemySpawner != null) enemySpawner.SetActive(false);

        StartCoroutine(GameSetupSequence());
    }

    IEnumerator GameSetupSequence()
    {
        Debug.Log("⏳ Waiting for countdown to finish...");
        yield return StartCoroutine(StartCountdown()); // ✅ Wait for countdown to complete

        Debug.Log("🎮 Starting game after countdown!");
        StartGame();
    }

  IEnumerator StartCountdown()
{
    yield return new WaitForSeconds(1f);

    if (countdownImage != null)
    {
        countdownImage.enabled = true;

        countdownImage.sprite = countdown3Sprite;
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownImage.sprite = countdown2Sprite;
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownImage.sprite = countdown1Sprite;
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownImage.sprite = goSprite;
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownImage.enabled = false;
        countdownPanel.SetActive(false);
    }
}


    void StartGame()
    {
        gameEnded = false;
        gameTime = 60f;
        
        if (trackmovement != null)
        {
            trackmovement.StartRunning();
        }

        if (ball != null)
        {
            ball.SetActive(true);
            Debug.Log("✅ Ball is now active!");
        }
        if(timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
        

        if (coinSpawner != null)
        {
            coinSpawner.SetActive(true);
            CoinSpawner spawner = coinSpawner.GetComponent<CoinSpawner>();
            if (spawner != null)
            {
            spawner.SpawnCoins();
            }
        }

        if (enemySpawner != null)
        {
            enemySpawner.SetActive(true);
            ObstacleSpawner objspawner = enemySpawner.GetComponent<ObstacleSpawner>();
            if (objspawner != null)
            {
                objspawner.StartSpawning();
                Debug.Log("✅ Enemy spawning started!");
            }
        }


        StartCoroutine(ScoreCountingRoutine());
    }

    void Update()
    {
        if (!gameEnded)
        {
            UpdateTimer();
        }
        else
        {
            ball.SetActive(false);
            enemySpawner.SetActive(false);
            coinSpawner.SetActive(false);
        }
    }

    IEnumerator ScoreCountingRoutine()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(scoreIncreaseRate);
            IncreaseScore(10);
        }
    }

    public void IncreaseScore(int amount)
    {
        if (gameEnded) return;
        score += amount;
        scoreText.text = "SCORE : " + score;
    }

    void UpdateTimer()
    {
        if (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60);
            int seconds = Mathf.FloorToInt(gameTime % 60);
            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
        else if (!gameEnded)
        {
            timerText.text = "00:00";
            GameWinActivated();
        }
    }

    public void ReduceLife()
    {
        if (gameEnded) return;

        if (playerLives > 0)
        {
            playerLives--;
            heartIcons[playerLives].enabled = false;
            if(gameplayed % 3 == 0)
            {
                AdsManager.Instance.interstitialAds.ShowInterstitialAd();
            }

           
        }

        if (playerLives <= 0)
        {
            if(gameplayed % 3 == 0)
            {
                AdsManager.Instance.interstitialAds.ShowInterstitialAd();
            }
            GameOverActivated();
        }
    }

    public void GameOverActivated()
    {
        gameEnded = true;
        AdsManager.Instance.bannerAds.HideBannerAd();
         if (trackmovement != null)
        trackmovement.isRunning = false;
        gameOverManager.ShowLosePanel(score);
    }

    public void GameWinActivated()
    {
        gameEnded = true;
        AdsManager.Instance.bannerAds.HideBannerAd();
         if (trackmovement != null)
        trackmovement.isRunning = false;

        StartCoroutine(ShowFinishThenWin());
    }

    IEnumerator ShowFinishThenWin()
    {
    if (finishPanel != null)
    {
        finishPanel.SetActive(true);
    }

    yield return new WaitForSeconds(finishPanelDelay);

    if (finishPanel != null)
    {
        finishPanel.SetActive(false); // Optional: hide finish panel before showing win panel
    }

    gameOverManager.ShowWinPanel(score);
}

    public void RestartGame()
    {
        gameplayed++;
        AdsManager.Instance.bannerAds.ShowBannerAd();
        Time.timeScale = 1; // Ensure time scale is reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("🚪 Game is quitting...");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1; // Ensure time scale is reset
        SceneManager.LoadScene("Scene 3 (Menu)"); // Replace with your main menu scene name
    }
    public void pauseGame()
    {
        Time.timeScale = 0; // Pause the game
    }
    public void resumeGame()
    {
        Time.timeScale = 1; // Resume the game
    }    
    
}
