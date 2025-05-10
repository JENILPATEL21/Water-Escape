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
        LoadLevel();
    }

    public void TouchControl()
    {
        IsTilt = false;
        LoadLevel();
    }

    private void LoadLevel()
    {
        Debug.Log("🔄 Loading Level...");
        SceneManager.LoadSceneAsync("Level1");
    }
}