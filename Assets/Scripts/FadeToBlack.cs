using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class FadeToBlack : MonoBehaviour
{
    public Animator fadeAnimator;
    public GameObject fadePanel;
    public float fadeOutDuration = 0.85f;

    void Awake()
    {
        if (fadeAnimator == null)
        {
            fadeAnimator = GetComponent<Animator>();
        }

        if (fadeAnimator == null)
        {
            fadeAnimator = GetComponentInChildren<Animator>();
        }
    }
    
    public void StartTransition(string sceneName = "Fight", Action onComplete = null)
    {
        StartCoroutine(Transition(sceneName, onComplete));
    }

    IEnumerator Transition(string sceneName, Action onComplete)
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(fadeOutDuration);
        }
        else
        {
            Debug.LogWarning("FadeToBlack: fadeAnimator is not assigned. Loading scene without fade animation.");
        }

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        load.allowSceneActivation = false;

        while (load.progress < 0.9f)
            yield return null;

        load.allowSceneActivation = true;

        while (!load.isDone)
            yield return null;

        yield return null;
        onComplete?.Invoke();
        
    }
}
