using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField classField;
    public TMP_InputField groupField;
    public TMP_InputField passwordField;
    [Space(5)]
    public Button submitButton;

    public TMPro.TextMeshProUGUI errorText;
    public GameObject registrationCanvas;
    public GameObject loginCanvas;

    public void CallRegister()
    {
        if (VerifyClassInput(classField.text))
            _ = StartCoroutine(RegisterCR());
        else
        {
            errorText.SetText("Enter valid class number. For example: 1A");
            errorText.gameObject.SetActive(true);
        }

        IEnumerator RegisterCR()
        {
            List<IMultipartFormSection> formData = new()
            {
                new MultipartFormDataSection(name: "name", data: nameField.text),
                new MultipartFormDataSection(name: "class", data: classField.text),
                new MultipartFormDataSection(name: "group", data: groupField.text),
                new MultipartFormDataSection(name: "password", data: passwordField.text)
            };

            using (var request = UnityWebRequest.Post(DBManager.phpFolderURL + "register.php", formData))
            {
                DownloadHandlerBuffer handler = new();
                request.downloadHandler = handler;

                yield return request.SendWebRequest();

                if (handler.text.StartsWith("0"))
                {
                    Debug.Log("User created succesfully!");
                    
                    DBManager.Singleton.LogIn(nameField.text, classField.text, 0, groupField.text);

                    registrationCanvas.SetActive(false);
                    loginCanvas.SetActive(true);
                    errorText.gameObject.SetActive(false);

                    nameField.text = "";
                    classField.text = "";
                    groupField.text = "";
                    passwordField.text = "";
                }
                else
                {
                    //Debug.Log(handler.text);
                    errorText.SetText(handler.text);
                    errorText.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Is everything filled in & with minimum lengths?
    /// </summary>
    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 4 && passwordField.text.Length >= 8 && classField.text.Length == 2);
    }

    /// <summary>
    /// Returns if the class input is in the format like '1A' / '4B' etc.
    /// </summary>
    /// <param name="input"></param>
    public bool VerifyClassInput(string input)
    {
        return char.IsNumber(input[0]) && char.IsLetter(input[1]);
    }

    public void ClassToUpperCase()
    {
        string toUpper = classField.text.ToUpper();
        classField.SetTextWithoutNotify(toUpper);
    }
}
