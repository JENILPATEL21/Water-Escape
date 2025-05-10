using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;
    public GameObject loginPanel;  // Assign in Inspector
    public GameObject menuPanel;   // Assign in Inspector
    public PlayerData playerData;

    void Start()
    {
        // Check if the user has already logged in before
        if (PlayerPrefs.GetInt("IsLoggedIn", 0) == 1)
        {
            loginPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
        else
        {
            loginPanel.SetActive(true);
            menuPanel.SetActive(false);
        }
    }

    public void OnLoginButton()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "admin" && password == "1234") // Replace with actual authentication
        {
            messageText.text = "Login Successful!";
            PlayerPrefs.SetString("Username", username); // Save login info
            PlayerPrefs.SetInt("IsLoggedIn", 1); // Save logged-in status as 1 (true)
            GameObject.Find("LoginPanel").SetActive(false);
            GameObject.Find("MenuPanel").SetActive(true);
        }
        else
        {
            messageText.text = "Invalid Username or Password!";
        }
    }
     public void ResetLoginAttempts()
    {
        playerData.loginAttempts = 0;  // Reset login attempts when needed (e.g., on a logout or new game start)
    }
}
