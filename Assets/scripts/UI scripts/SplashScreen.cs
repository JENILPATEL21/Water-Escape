using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{

    void Awake()
    {
        Application.runInBackground = false;
    }


    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3); // Adjust delay as needed
        SceneManager.LoadScene("LoadingS");
    }
    
    IEnumerator ShowBannerAd()
    {
        yield return new WaitForSeconds(4);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }
}

