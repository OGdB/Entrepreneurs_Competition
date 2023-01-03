using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnEnter : MonoBehaviour
{
    private Button thisButton;
    private void Start() => thisButton = GetComponent<Button>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && thisButton.interactable)
            thisButton.onClick?.Invoke();
    }

}
