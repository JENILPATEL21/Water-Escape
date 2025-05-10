using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Animator fadeAnimator;

    public void FadeIn()
    {
        fadeAnimator.SetTrigger("StartFadeIn");
    }

    public void FadeOut()
    {
        fadeAnimator.SetTrigger("StartFadeOut");
    }
}
