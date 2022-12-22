using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Registration : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField classField;
    public TMP_InputField passwordField;
    [Space(5)]
    public Button submitButton;

    public void CallRegister()
    {
        if (VerifyClassInput(classField.text))
            _ = StartCoroutine(RegisterCR());
        else
            print("Enter valid class number. For example: 1A");

        IEnumerator RegisterCR()
        {
            List<IMultipartFormSection> formData = new();
            formData.Add(new MultipartFormDataSection(name: "name", data: nameField.text));
            formData.Add(new MultipartFormDataSection(name: "class", data: classField.text));
            formData.Add(new MultipartFormDataSection(name: "password", data: passwordField.text));

            using (var request = UnityWebRequest.Post(DBManager.phpFolderURL + "register.php", formData))
            {
                DownloadHandlerBuffer handler = new();
                request.downloadHandler = handler;

                yield return request.SendWebRequest();

                if (handler.text.StartsWith("0"))
                {
                    Debug.Log("User created succesfully!");
                    
                    DBManager.UserName = nameField.text;
                    DBManager.ClassNumber = classField.text;
                    DBManager.Score = 0;

                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
                else
                {
                    Debug.Log($"User creation failed!\n" +
                        $"Error #{handler.text}");
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
