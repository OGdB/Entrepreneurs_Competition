using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OnUIInteractableInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Properties
    [SerializeField, Tooltip("The color to change to when the toggle is unselected.")]
    private Color32 _onUnselectedColor = new(255, 255, 255, 255);
    [SerializeField, Tooltip("The color to change to when the toggle is hovered over.")]
    private Color32 _onHoverColor = new(100, 213, 255, 255);
    [SerializeField, Tooltip("The color to change to when the toggle is selected.")]
    private Color32 _onSelectedColor = new(100, 213, 255, 255);

    [SerializeField, Tooltip("Should there be a smooth color transition, or should the change be instant?")]
    private bool fadeTransition = true;
    [SerializeField, Tooltip("The speed in seconds at which the color fade transition occurs.")]
    private float fadeTransitionSpeed = 0.15f;
    private Coroutine transitionCoroutine;
    private bool isTransitioning = false;

    private Image _image;
    #endregion

    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// The Toggle was clicked, change color accordingly.
    /// </summary>
    /// <param name="state"></param>
    protected virtual void InteractedWithUI(bool state)
    {
        if (state == true)
        {
            _image.color = _onSelectedColor;
        }
        else
        {
            _image.color = _onUnselectedColor;
        }
    }

    /// <summary>
    /// Started hovering
    /// </summary>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {

        if (fadeTransition)
        {
            if (isTransitioning) StopCoroutine(transitionCoroutine);

            transitionCoroutine = StartCoroutine(FadeColor(_onHoverColor));
        }
        else
        {
            _image.color = _onHoverColor;
        }
    }

    /// <summary>
    /// Stopped hovering
    /// </summary>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (fadeTransition)
        {
            if (isTransitioning) StopCoroutine(transitionCoroutine);

            transitionCoroutine = StartCoroutine(FadeColor(_onUnselectedColor));
        }
        else
        {
            _image.color = _onUnselectedColor;
        }
    }

    /// <summary>
    /// Lerps the color of the button to the target color in the "fadeTransitionSpeed" amount of seconds;
    /// </summary>
    /// <param name="targetColor"></param>
    protected virtual IEnumerator FadeColor(Color32 targetColor)
    {
        isTransitioning = true;
        Color initialColor = _image.color;

        float elapsedTime = 0f;

        while (elapsedTime < fadeTransitionSpeed)
        {
            elapsedTime += Time.deltaTime;
            _image.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeTransitionSpeed);
            yield return null;
        }

        isTransitioning = false;
    }

    public virtual void ResetToggle()
    {
        GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        _image.color = _onUnselectedColor;
    }
}
