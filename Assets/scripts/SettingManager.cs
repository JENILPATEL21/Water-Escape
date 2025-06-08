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
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 0);
        SetDifficultyUI(savedDifficulty);

        easyButton.onClick.AddListener(() => OnDifficultySelected(0));
        hardButton.onClick.AddListener(() => OnDifficultySelected(1));
        proButton.onClick.AddListener(() => OnDifficultySelected(2));
    }

    private void OnDifficultySelected(int difficulty)
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.Save();

        SetDifficultyUI(difficulty);

        // Load Control Scene (mandatory)
      //  SceneTransitionManager.Instance.LoadSceneWithFade("ControlScene"); 
    }

    private void SetDifficultyUI(int difficulty)
    {
        easyIndicator.sprite = crossSprite;
        hardIndicator.sprite = crossSprite;
        proIndicator.sprite = crossSprite;

        if (difficulty == 0) easyIndicator.sprite = checkSprite;
        else if (difficulty == 1) hardIndicator.sprite = checkSprite;
        else if (difficulty == 2) proIndicator.sprite = checkSprite;

        Debug.Log("Selected Difficulty: " + (difficulty == 0 ? "Easy" : difficulty == 1 ? "Hard" : "Pro"));
    }
}
