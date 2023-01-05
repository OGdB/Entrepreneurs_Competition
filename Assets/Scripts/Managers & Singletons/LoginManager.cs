using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    [Space(5)]
    public Button submitButton;

    public void CallLogin()
    {
        if (DBManager.LoggedIn(nameField.text)) return;

        _ = StartCoroutine(LoginCR(nameField.text, passwordField.text, true));
    }

    public static IEnumerator LoginCR(string username, string password, bool switchToMainMenu = false)
    {
        List<IMultipartFormSection> formData = new()
            {
                new MultipartFormDataSection(name: "name", data: username),
                new MultipartFormDataSection(name: "password", data: password)
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

            DBManager.Singleton.LogIn(username, classNumber, score, groupName);

            if (switchToMainMenu)
                SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log($"User login failed. Error #{handler.text}");
        }
    }

    public bool AreConditionsMet() => (nameField.text.Length >= 4 && passwordField.text.Length >= 8);

    public void VerifyInputs() => submitButton.interactable = AreConditionsMet();
}
