using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f;

    // Make sure this manager is only created once
    private static SceneTransitionManager instance;

    public static SceneTransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneTransitionManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("SceneTransitionManager");
                    instance = obj.AddComponent<SceneTransitionManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    // This is to make sure no duplicate managers are created
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Method to load scene with transition
    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        // Fade out animation
        transitionAnimator.SetTrigger("StartFadeOut");

        // Wait for fade-out animation to complete
        yield return new WaitForSeconds(transitionTime);

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Wait a bit to let the scene load (can adjust if needed)
        yield return new WaitForSeconds(0.1f);

        // Fade in animation
        transitionAnimator.SetTrigger("StartFadeIn");
    }
}
