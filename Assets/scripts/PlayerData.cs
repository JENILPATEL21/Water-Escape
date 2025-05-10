using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public string username;           // Username
    public string password;           // Password
    public int loginAttempts = 0;     // Number of login attempts
    public int finalScore = 0;        // Final score from the game

    public int lives = 3;  // Player starts with 3 lives
    public int winTime = 60; // Time to win the game
}
