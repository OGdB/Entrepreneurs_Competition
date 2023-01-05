using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Performs the 'OnClick' callbacks of the button this is on when pressing Enter/Return
/// </summary>
[RequireComponent(typeof(Button))]
public class OnEnter : MonoBehaviour
{
    Button thisButton;
    private void Start() => thisButton = GetComponent<Button>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && thisButton.interactable)
            thisButton.onClick.Invoke();
    }

}
