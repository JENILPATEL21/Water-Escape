using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public BannerAds bannerAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;

    public static AdsManager Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    bool noAds = PlayerPrefs.GetInt("NoAds", 0) == 1;

    if (!noAds)
    {
        bannerAds.LoadBannerAd();
        interstitialAds.LoadInterstitialAd();
    }
        rewardedAds.LoadRewardedAd();
    }
    public void HideBanner()
{
    bannerAds.HideBannerAd();
}

}