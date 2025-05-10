using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // fixed here
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // and fixed here
        StartCoroutine(DisplayBannerWithDelay());
    }

    IEnumerator DisplayBannerWithDelay()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    void Update()
    {

    }
}
