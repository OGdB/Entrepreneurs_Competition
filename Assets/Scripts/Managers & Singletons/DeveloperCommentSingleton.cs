using UnityEngine;

public class DeveloperCommentSingleton : MonoBehaviour
{
    public static DeveloperCommentSingleton Singleton;

    public Canvas Canvas { get => canvas; set => canvas = value; }
    private Canvas canvas;

    public TMPro.TextMeshProUGUI developerText;

    private void Awake()
    {
        if (!Singleton)
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
    private void OnDestroy()
    {
        if (Singleton == this)
            Singleton = null;
    }
    private void Start()
    {
        Canvas = GetComponent<Canvas>();
    }
    public static void OnTriggerEntered()
    {
        Singleton.canvas.enabled = true;
    }
    public static void OnTriggerExited()
    {
        Singleton.canvas.enabled = false;
    }
    public static void SetDeveloperComment(string comment)
    {
        Singleton.developerText.SetText(comment);
    }
}
