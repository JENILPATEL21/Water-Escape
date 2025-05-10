using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject winPanel;     // Win Panel
    public GameObject losePanel;    // Lose Panel
    public float delayTime = 2.0f;  // Delay before showing score

    public void ShowWinPanel(int finalScore)
    {
        StartCoroutine(ShowGamePanel(winPanel, finalScore));
    }

    public void ShowLosePanel(int finalScore)
    {
        StartCoroutine(ShowGamePanel(losePanel, finalScore));
    }

    private IEnumerator ShowGamePanel(GameObject panel, int finalScore)
    {
        if (panel == null)
        {
            Debug.LogError("GameOverManager: Panel not assigned!");
            yield break;
        }

        panel.SetActive(true);  // Enable the panel
        GameObject symbol = panel.transform.GetChild(0).gameObject; // First child (symbol)
        GameObject scoreObject = panel.transform.GetChild(1).gameObject; // Second child (score display)

        if (symbol == null || scoreObject == null)
        {
            Debug.LogError("GameOverManager: Panel children not found!");
            yield break;
        }

        Text finalScoreText = scoreObject.GetComponentInChildren<Text>();

        symbol.SetActive(true);  // Show symbol
        scoreObject.SetActive(false); // Hide score initially

        yield return new WaitForSeconds(delayTime); // Wait for delay

        symbol.SetActive(false);
        scoreObject.SetActive(true);  // Show score display

        if (finalScoreText != null)
        {
            finalScoreText.text =  finalScore.ToString(); // Update score text
        }
        else
        {
            Debug.LogError("GameOverManager: Final score text field not found!");
        }

        yield return new WaitForSeconds(1f); // Short delay before stopping time

        Time.timeScale = 0;  // Stop time AFTER showing the score
    }
}
