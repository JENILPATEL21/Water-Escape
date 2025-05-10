using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;       // Assign this in the Inspector
    public float loadingSpeed = 0.5f;

    private int nextSceneIndex;

    void Start()
    {
        // Get the next scene index based on current one
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex + 1;

        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneIndex);
        operation.allowSceneActivation = false;

        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * loadingSpeed;
            loadingBar.value = progress;

            if (progress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
