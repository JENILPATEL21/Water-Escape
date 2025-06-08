using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class uiManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public Image[] heartIcons;
    public GameObject GamePanel;

    [Header("Countdown")]
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;
    public Image countdownImage;
    public Sprite countdown3;
    public Sprite countdown2;
    public Sprite countdown1;
    public Sprite countdownGo;

    [Header("Gameplay Elements")]
    public GameObject ball;
    public GameObject enemySpawner;
    public GameObject coinSpawner;
    public TrackLooper trackmovement;

    [Header("Finish Game Panel")]
    public GameObject finishPanel;
    public float finishPanelDelay = 2f;

    [Header("Game Settings")]
    public float gameTime = 60f;
    public float scoreIncreaseRate = 1f;

    [Header("Hit Effect")]
    public Image hitFlashImage;
    public float flashDuration = 0.5f;
    public Camera mainCam;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;

    [Header("Distance")]
    public TextMeshProUGUI distanceText;
    [Tooltip("Metres gained per real-time second")]
    public float distanceMultiplier;

    [Header("Milestone Popup")]
    public GameObject milestoneIndicator;
    public int playerLives;

    private int score;
    private Coroutine scoreRoutine;
    private GameOverManager gameOverManager;
    private bool gameRunning = false;
    public bool gameEnded = false;

    private bool distanceCountingStarted = false;
    private float distanceTraveled = 0f;
    private readonly List<int> milestoneList = new() {500, 1000, 5000, 10000 };
    private readonly HashSet<int> triggeredMilestones = new();

    private void Awake()
    {
        gameOverManager = FindObjectOfType<GameOverManager>(true);
        if (gameOverManager == null)
            Debug.LogError("GameOverManager not found in scene!");
    }

    private void Start()
    {
        playerLives = heartIcons.Length;

        scoreText.text = "SCORE : 0";
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        countdownPanel?.SetActive(true);
        ball?.SetActive(false);
        enemySpawner?.SetActive(false);
        coinSpawner?.SetActive(false);

        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if (gameEnded || !gameRunning) return;

        gameTime -= Time.deltaTime;
        if (gameTime > 0f)
        {
            int m = Mathf.FloorToInt(gameTime / 60f);
            int s = Mathf.FloorToInt(gameTime % 60f);
            timerText.text = $"{m:D2}:{s:D2}";
        }
        else
        {
            timerText.text = "00:00";
            GameWinActivated();
            return;
        }

        if (ball != null)
        {
            distanceTraveled += Time.deltaTime * distanceMultiplier;
            distanceText.text = $"{Mathf.FloorToInt(distanceTraveled)}m";

            foreach (int milestone in milestoneList)
            {
                if (distanceTraveled >= milestone && !triggeredMilestones.Contains(milestone))
                {
                    triggeredMilestones.Add(milestone);
                    StartCoroutine(ShowMilestoneAchieved(milestone));
                }
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);
        countdownImage.sprite = countdown3; countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownImage.sprite = countdown2; countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownImage.sprite = countdown1; countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownImage.sprite = countdownGo; countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownPanel?.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        gameEnded = false;
        gameRunning = true;
        distanceCountingStarted = true;

        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        GamePanel.gameObject.SetActive(true);

        trackmovement?.StartRunning();
        ball?.SetActive(true);

        coinSpawner?.SetActive(true);
        coinSpawner?.GetComponent<CoinSpawner>()?.SpawnCoins();

        enemySpawner?.SetActive(true);
        enemySpawner?.GetComponent<ObstacleSpawner>()?.StartSpawning();

        if (scoreRoutine != null) StopCoroutine(scoreRoutine);
        scoreRoutine = StartCoroutine(IncreaseScoreOverTime());
    }

    private IEnumerator IncreaseScoreOverTime()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(scoreIncreaseRate);
            IncreaseScore(10);
        }
    }

     public void LoadNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";

        switch (currentScene.ToLower())
        {
            case "easy":
                nextScene = "Hard";
                break;
            case "hard":
                nextScene = "Pro";
                break;
            case "pro":
                nextScene = "Easy";
                break;
            default:
                Debug.LogWarning("Unknown scene. Reloading current scene.");
                nextScene = currentScene;
                break;
        }

        SceneManager.LoadScene(nextScene);
    }

    public void IncreaseScore(int amount)
    {
        if (gameEnded) return;
        score += amount;
        scoreText.text = $"SCORE : {score}";
    }

    public void ReduceLife()
    {
        if (gameEnded) return;

        playerLives--;
        if (playerLives >= 0 && playerLives < heartIcons.Length)
            heartIcons[playerLives].enabled = false;

        StartCoroutine(FlashRed());
        StartCoroutine(CameraShake());

        if (playerLives <= 0)
            GameOverActivated();
    }

    private IEnumerator FlashRed()
    {
        if (!hitFlashImage) yield break;

        Color col = hitFlashImage.color;
        col.a = 1f;
        hitFlashImage.color = col;

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            col.a = Mathf.Lerp(1f, 0f, t / flashDuration);
            hitFlashImage.color = col;
            yield return null;
        }

        col.a = 0f;
        hitFlashImage.color = col;
    }

    private IEnumerator CameraShake()
    {
        if (!mainCam) yield break;

        Vector3 original = mainCam.transform.position;
        float t = 0f;

        while (t < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            mainCam.transform.position = original + new Vector3(x, y, 0f);

            t += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = original;
    }

    private IEnumerator ShowMilestoneAchieved(int milestone)
    {
        if (!milestoneIndicator) yield break;

        milestoneIndicator.SetActive(true);

        TextMeshProUGUI txt = milestoneIndicator.GetComponentInChildren<TextMeshProUGUI>();
        if (txt) txt.text = $"{milestone}m Passed!";

        yield return new WaitForSeconds(2f);
        milestoneIndicator.SetActive(false);
    }

    public void GameOverActivated()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameRunning = false;
        GamePanel.gameObject.SetActive(false);
         if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGameOverSFX();
            }

        trackmovement?.StopRunning();
        enemySpawner?.SetActive(false);
        coinSpawner?.SetActive(false);
        if (scoreRoutine != null) StopCoroutine(scoreRoutine);

        gameOverManager?.ShowLosePanel(score);
    }

    public void GameWinActivated()
    {
        if (gameEnded) return;
        gameEnded = true;
        gameRunning = false;
        GamePanel.gameObject.SetActive(false);
         if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameWonSFX();
        }
        trackmovement?.StopRunning();
        enemySpawner?.SetActive(false);
        coinSpawner?.SetActive(false);
        if (scoreRoutine != null) StopCoroutine(scoreRoutine);

        StartCoroutine(ShowFinishPanelThenWin());
    }

    private IEnumerator ShowFinishPanelThenWin()
    {
        finishPanel?.SetActive(true);
        yield return new WaitForSecondsRealtime(finishPanelDelay);
        finishPanel?.SetActive(false);

        gameOverManager?.ShowWinPanel(score);
    }

    public void RestartGame()
    {
    Time.timeScale = 1f;
    // Use fade transition instead of immediate load
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

public void QuitGame() => Application.Quit();

public void PauseGame() => Time.timeScale = 0f;

public void ResumeGame() => Time.timeScale = 1f;

public void LoadMenuScene()
{
    Time.timeScale = 1f;
    // Use fade transition to load menu scene by build index or name
    SceneManager.LoadScene("MenuS");
    // Or if you prefer using build index:
    // SceneTransitionManager.Instance.LoadSceneWithFade(SceneManager.GetSceneByBuildIndex(2).name);
}


}
