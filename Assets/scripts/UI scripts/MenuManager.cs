using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject loginPanel;     // Assign in Inspector
    public GameObject mainMenuPanel;

    int currentSceneIndex;  // ✅ Declare only, don't assign here!

    void Awake()
    {
        // ✅ Safe place to access SceneManager
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
       /* if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            ShowMainMenu();
        }
        else
        {
            ShowLogin();
        } */

        ShowMainMenu();
    }

    public void GoToLogin()
    {
        PlayerPrefs.SetInt("IsLoggedIn", 0);
        ShowLogin();
    }

    public void ChooseMode()
    {
        SceneManager.LoadScene("ControlS");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void ShowLogin()
    {
        loginPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void ShowMainMenu()
    {
        loginPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
