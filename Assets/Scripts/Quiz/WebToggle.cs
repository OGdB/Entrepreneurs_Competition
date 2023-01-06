using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class WebToggle : OnUIInteractableInteraction
{
    #region Properties
    private Toggle _toggle;
    [SerializeField]
    private GameObject line;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(InteractedWithUI);
    }
    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveAllListeners();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!_toggle.isOn)
            base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!_toggle.isOn)
            base.OnPointerExit(eventData);
    }
    protected override void InteractedWithUI(bool state)
    {
        base.InteractedWithUI(state);
        line.SetActive(state);
    }

    public override void ResetToggleSelectState()
    {
        base.ResetToggleSelectState();
        line.SetActive(false);
    }
}
