using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    [Space(5)]
    public Button submitButton;

    public void CallLogin()
    {
        // Is player name in namefield already lggged in?
        if (PlayerManager.Singleton.Group1 != null)
        {
            if (PlayerManager.Singleton.Group1.Contains(nameField.text))
            {
                print("This user is already logged in!");
                return;
            }
        }
        else if (PlayerManager.Singleton.Group2 != null)
        {
            if (PlayerManager.Singleton.Group2.Contains(nameField.text))
            {
                print("This user is already logged in!");
                return;
            }
        }

        _ = StartCoroutine(LoginCR());

        IEnumerator LoginCR()
        {
            List<IMultipartFormSection> formData = new()
            {
                new MultipartFormDataSection(name: "name", data: nameField.text),
                new MultipartFormDataSection(name: "password", data: passwordField.text)
            };

            using var request = UnityWebRequest.Post(DBManager.phpFolderURL + "login.php", formData);

            DownloadHandlerBuffer handler = new();
            request.downloadHandler = handler;

            yield return request.SendWebRequest();

            if (handler.text.StartsWith("0"))
            {
                string text = handler.text;
                string[] splitText = text.Split("\t");

                string classNumber = splitText[1];
                string groupName = "No group";
                if (splitText[2].Length > 0)
                    groupName = splitText[2];

                int score = -1;
                if (splitText[3].Length > 0)
                    score = int.Parse(splitText[3]);

                DBManager.LogIn(nameField.text, classNumber, score, groupName);

                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
            }
            else
            {
                Debug.Log($"User login failed. Error #{handler.text}");
            }
        }
    }

    public bool AreConditionsMet() => (nameField.text.Length >= 4 && passwordField.text.Length >= 8);

    public void VerifyInputs() => submitButton.interactable = AreConditionsMet();
}
