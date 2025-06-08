using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    public static ControlManager Instance { get; private set; }

    private static bool _isTilt = true;
    public static bool IsTilt
    {
        get => _isTilt;
        set
        {
            _isTilt = value;
            Debug.Log("✅ Control Mode Changed: " + (_isTilt ? "Tilt" : "Touch"));
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("🎮 ControlManager Initialized. Current Mode: " + (IsTilt ? "Tilt" : "Touch"));
    }

    public void TiltControl()
    {
        IsTilt = true;
        LoadGameSceneBasedOnDifficulty();
    }

    public void TouchControl()
    {
        IsTilt = false;
        LoadGameSceneBasedOnDifficulty();
    }

    private void LoadGameSceneBasedOnDifficulty()
    {
        int difficulty = PlayerPrefs.GetInt("Difficulty", 0);

        string sceneToLoad = difficulty switch
        {
            0 => "Easy",
            1 => "Hard",
            2 => "Pro",
            _ => "Easy"
        };

        Debug.Log($"🔄 Loading Game Scene for difficulty: {difficulty}, scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
