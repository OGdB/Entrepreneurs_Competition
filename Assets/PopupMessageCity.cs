using System.Collections;
using UnityEngine;

public class PopupMessageCity : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 0.4f;
    public TMPro.TextMeshProUGUI popupText;
    public CanvasGroup canvas;

    public void SetPopup(string text, float length)
    {
        _ = StartCoroutine(PopUpCR(text, length));
    }

    public IEnumerator PopUpCR(string text, float length)
    {
        popupText.text = text;

        float startTime = Time.time;
        float start = canvas.alpha;
        float target = 1f;
        float progress = 0f;

        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / fadeSpeed;

            float newAlpha = Mathf.Lerp(start, target, progress);

            canvas.alpha = newAlpha;

            yield return null;
        }

        yield return new WaitForSeconds(length);

        progress = 0f;
        target = 0f;
        startTime = Time.time;
        start = canvas.alpha;

        while (progress <= 1f)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = timeSinceStarted / fadeSpeed;

            float newAlpha = Mathf.Lerp(start, target, progress);

            canvas.alpha = newAlpha;

            yield return null;
        }
    }
}
