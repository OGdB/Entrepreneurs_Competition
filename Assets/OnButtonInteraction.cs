using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnButtonInteraction : OnUIInteractableInteraction
{
    #region Properties
    private Toggle _toggle;
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
}
