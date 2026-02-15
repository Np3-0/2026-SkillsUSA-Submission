using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class FadeToBlack : MonoBehaviour
{
    public Animator fadeAnimator;
    public GameObject fadePanel;
    
    public void StartTransition(string sceneName = "Fight", Action onComplete = null)
    {
        StartCoroutine(Transition(sceneName, onComplete));
    }

    IEnumerator Transition(string sceneName, Action onComplete)
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(.85f);

        onComplete?.Invoke();

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        load.allowSceneActivation = false;

        while (load.progress < 0.9f)
            yield return null;

        load.allowSceneActivation = true;
        
    }
}
