using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    int currentSceneIndex;

    void Awake()
    {
        Application.runInBackground = false;
    }


    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3); // Adjust delay as needed
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    
    IEnumerator ShowBannerAd()
    {
        yield return new WaitForSeconds(4);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }
}

