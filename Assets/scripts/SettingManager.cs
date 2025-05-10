using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Difficulty Buttons")]
    public Button easyButton;
    public Button hardButton;
    public Button proButton;

    [Header("Difficulty Indicators")]
    public Image easyIndicator;
    public Image hardIndicator;
    public Image proIndicator;

    public Sprite checkSprite; // Tick (✓)
    public Sprite crossSprite; // Cross (✖)

    private void Start()
    {
        // Load saved difficulty
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 0);
        SetDifficulty(savedDifficulty);

        // Add button click listeners
        easyButton.onClick.AddListener(() => SetDifficulty(0));
        hardButton.onClick.AddListener(() => SetDifficulty(1));
        proButton.onClick.AddListener(() => SetDifficulty(2));
    }

    private void SetDifficulty(int difficulty)
    {
        // Save selected difficulty
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.Save();

        // Reset all to ✖ (cross)
        easyIndicator.sprite = crossSprite;
        hardIndicator.sprite = crossSprite;
        proIndicator.sprite = crossSprite;

        // Set selected difficulty to ✓ (check)
        if (difficulty == 0) easyIndicator.sprite = checkSprite;
        else if (difficulty == 1) hardIndicator.sprite = checkSprite;
        else if (difficulty == 2) proIndicator.sprite = checkSprite;

        Debug.Log("Selected Difficulty: " + (difficulty == 0 ? "Easy" : difficulty == 1 ? "Hard" : "Pro"));
    }
}
