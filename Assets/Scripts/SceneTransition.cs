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
        }
        else
        {
            Destroy(gameObject);
        }

        GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }

    private void Start()
    {
        if (fadeOutCanvasGroup == null)
            fadeOutCanvasGroup = GetComponent<CanvasGroup>();
    }

    public static void TransitionToScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) => 
        Singleton.StartCoroutine(Singleton.WaitForFade(sceneName, mode));
    public static void TransitionToScene(int sceneInt, LoadSceneMode mode = LoadSceneMode.Single) => 
        Singleton.StartCoroutine(Singleton.WaitForFade(sceneInt, mode));

    private IEnumerator WaitForFade(string sceneName, LoadSceneMode mode)
    {
        if (isInTransition) yield break;

        EventSystem.current.enabled = false;
        isInTransition = true;

        yield return Fade(1f);

        SceneManager.LoadScene(sceneName, mode);

        yield return new WaitForSeconds(inBetweenWait);

        yield return Fade(0f);

        isInTransition = false;
        EventSystem.current.enabled = true;
    }
    private IEnumerator WaitForFade(int sceneInt, LoadSceneMode mode)
    {
        if (isInTransition) yield break;

        EventSystem.current.enabled = false;
        isInTransition = true;

        yield return Fade(1f);

        SceneManager.LoadScene(sceneInt, mode);

        yield return new WaitForSeconds(inBetweenWait);

        yield return Fade(0f);

        GetComponentInParent<Canvas>().worldCamera = Camera.main;
        isInTransition = false;
        EventSystem.current.enabled = true;
    }

    public static IEnumerator Fade(float targetAlpha)
    {
        float startTime = Time.time;

        float start = Singleton.fadeOutCanvasGroup.alpha;
        float target = targetAlpha;

        float progress = 0f;

        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / Singleton.fadeOutSpeed;

            float newAlpha = Mathf.Lerp(start, target, progress);
            
            Singleton.fadeOutCanvasGroup.alpha = newAlpha;

            yield return null;
        }
    }
}
