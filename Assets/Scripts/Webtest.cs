using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Webtest : MonoBehaviour
{
    public int score = 6969;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetScore(score.ToString()));
        }

        IEnumerator GetScore(string score)
        {
            if (!DBManager.LoggedIn)
            {
                print("Log in first");
                yield break;
            }

            List<IMultipartFormSection> formData = new();
            formData.Add(new MultipartFormFileSection("name", DBManager.UserName));
            print(DBManager.UserName);
            formData.Add(new MultipartFormFileSection("score", score));

            using (var request = UnityWebRequest.Post(DBManager.phpFolderURL + "webtest.php", formData))
            {
                DownloadHandlerBuffer handler = new DownloadHandlerBuffer();
                request.downloadHandler = handler;

                yield return request.SendWebRequest();

                if (handler.text.StartsWith("0")) // handler.text should return 0 if everything went correctly.
                {
                    Debug.Log(handler.text);
                }
                else
                {
                    Debug.Log($"Retrieving data failed. Error #{handler.text}");
                }
            }
        }
    }
}
