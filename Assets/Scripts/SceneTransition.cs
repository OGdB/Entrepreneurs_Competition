using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Singleton;

    public float fadeOutSpeed = 0.8f; // Speed in seconds the screen fades in- and out.
    public float inBetweenWait = 0.5f; // Short interval between transitioning in again.
    public CanvasGroup fadeOutCanvasGroup;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (fadeOutCanvasGroup == null)
            fadeOutCanvasGroup = GetComponent<CanvasGroup>();
    }

    public static void TransitionToScene(string sceneName)
    {
        _ = Singleton.StartCoroutine(WaitForFade());

        IEnumerator WaitForFade()
        {
            yield return Singleton.Fade(1f);

            SceneManager.LoadScene(sceneName);

            yield return new WaitForSeconds(Singleton.inBetweenWait);

            yield return Singleton.Fade(0f);

            print("Scene Transition finished");
        }
    }
    public static void TransitionToScene(int sceneInt)
    {
        _ = Singleton.StartCoroutine(WaitForFade());

        IEnumerator WaitForFade()
        {
            yield return Singleton.Fade(1f);

            SceneManager.LoadScene(sceneInt);

            yield return new WaitForSeconds(Singleton.inBetweenWait);

            yield return Singleton.Fade(0f);

            print("Scene Transition finished");
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startTime = Time.time;

        float start = targetAlpha == 1f? 0f : 1f;
        float target = targetAlpha;

        float progress = 0f;

        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / fadeOutSpeed;

            float newAlpha = Mathf.Lerp(start, target, progress);
            
            fadeOutCanvasGroup.alpha = newAlpha;

            yield return null;
        }
    }
}
