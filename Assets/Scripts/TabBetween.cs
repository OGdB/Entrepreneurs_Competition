using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class TabBetween : MonoBehaviour
{
    private TMP_InputField myField;
    public TMP_InputField nextField;

    private void Start()
    {
        if (nextField == null)
        {
            Destroy(this);
            return;
        }
        
        myField = GetComponent<TMP_InputField>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && myField.isFocused)
        {
            nextField.ActivateInputField();
        }
    }
}
