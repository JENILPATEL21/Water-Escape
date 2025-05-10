using UnityEngine;
using System.Collections;

public class trackMove : MonoBehaviour 
{
     float speed = 0.1f;
    private Vector2 offset;
    private uiManager ui; // ✅ Reference to UI Manager

    void Start() 
    {
        Screen.SetResolution((int)Screen.width, (int)Screen.height, true);  
        ui = FindObjectOfType<uiManager>(); // ✅ Find UI Manager in the scene
    }

    void Update() 
{
    if (ui == null) return;

    // ⛔ Stop track movement if game has ended or countdown is active
    if (ui.gameEnded || ui.countdownPanel.activeSelf) 
    {
        return;
    }

    offset = new Vector2(0, Time.time * speed);
    GetComponent<Renderer>().material.mainTextureOffset = offset;
}

}
