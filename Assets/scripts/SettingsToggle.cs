using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject bt1;
    public GameObject bt2;
    

    public void ToggleSettings()
    {
        // Toggle the active state of the settings panel
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        bt1.SetActive(!bt1.activeSelf);
        bt2.SetActive(!bt2.activeSelf);
    }
}
