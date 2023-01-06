using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Singleton;

    public float fadeOutSpeed = 0.8f; // Speed in seconds the screen fades in- and out.
    public float inBetweenWait = 0.5f; // Short interval between transitioning in again.
    public CanvasGroup fadeOutCanvasGroup;
    private bool isInTransition = false;

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

    public static void TransitionToScene(string sceneName) => 
        Singleton.StartCoroutine(Singleton.WaitForFade(sceneName));
    public static void TransitionToScene(int sceneInt) => 
        Singleton.StartCoroutine(Singleton.WaitForFade(sceneInt));

    private IEnumerator WaitForFade(string sceneName)
    {
        if (isInTransition) yield break;

        EventSystem.current.enabled = false;
        isInTransition = true;

        yield return Fade(1f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(inBetweenWait);

        yield return Fade(0f);

        isInTransition = false;
        EventSystem.current.enabled = true;
    }
    private IEnumerator WaitForFade(int sceneInt)
    {
        if (isInTransition) yield break;

        EventSystem.current.enabled = false;
        isInTransition = true;

        yield return Fade(1f);

        SceneManager.LoadScene(sceneInt);

        yield return new WaitForSeconds(inBetweenWait);

        yield return Fade(0f);

        isInTransition = false;
        EventSystem.current.enabled = true;
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
