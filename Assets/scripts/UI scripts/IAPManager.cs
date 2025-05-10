using UnityEngine;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    public Button removeAdsButton;

    void Start()
    {
        // Check if ads were already removed
        if (PlayerPrefs.GetInt("NoAds", 0) == 1)
        {
            removeAdsButton.gameObject.SetActive(false);
        }

        // Optional: hook up listener in code
        if (removeAdsButton != null)
        {
            removeAdsButton.onClick.AddListener(SimulateRemoveAdsPurchase);
        }
    }

    public void SimulateRemoveAdsPurchase()
    {
        Debug.Log("🛒 Simulating purchase: Remove Ads");

        // Save preference locally
        PlayerPrefs.SetInt("NoAds", 1);
        PlayerPrefs.Save();

        Debug.Log("✅ Purchase simulated. Ads removed!");
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.HideBanner();
        }

        removeAdsButton.gameObject.SetActive(false);

        Debug.Log("✅ Remove Ads Purchased (Simulated)");
    }
}
