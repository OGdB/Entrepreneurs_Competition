using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Put on a TMPro.UGUI element to make it show the user who's turn it is.
/// </summary>
public class CurrentUser : MonoBehaviour
{
    public static CurrentUser Singleton;

    private float animationLength = 0.5f;

    private WaitForEndOfFrame frame;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(transform.parent.parent.gameObject);
        }
        else
        {
            Destroy(transform.parent.parent.gameObject);
        }

        frame = new();
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
        SetToCurrentUser();
    }



    private void OnEnable()
    {
        DBManager.OnCurrentUserChanged += SetToCurrentUser;
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene arg0, Scene arg1)
    {
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }

    private void OnDisable()
    {
        DBManager.OnCurrentUserChanged -= SetToCurrentUser;
        SceneManager.activeSceneChanged -= ChangedActiveScene;
    }

    private void SetToCurrentUser()
    {
        GetComponent<TMPro.TextMeshProUGUI>().SetText(DBManager.Singleton.currentUser.Name);
        _ = StartCoroutine(ChangePlayerAnimation());
    }

    private IEnumerator ChangePlayerAnimation()
    {
        float startTime = Time.time;
        float halfAnimation = animationLength / 2f;

        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.one * 1.3f;

        float progress = 0f;
        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / halfAnimation;
            transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return frame;
        }

        startTime = Time.time;
        progress = 0f;
        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / halfAnimation;
            transform.localScale = Vector3.Lerp(targetScale, startScale, progress);
            yield return frame;
        }
    }
}
