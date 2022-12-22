using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Vote
{
    IEnumerator VoteForOption(string answer, string question)
    {
        List<IMultipartFormSection> formData = new();
        formData.Add(new MultipartFormFileSection("name", DBManager.UserName));
        formData.Add(new MultipartFormFileSection("answer", answer));

        using (var request = UnityWebRequest.Post("https://oeds.online/votesystem.php", formData))
        {
            DownloadHandlerBuffer handler = new();
            request.downloadHandler = handler;

            yield return request.SendWebRequest();

            if (handler.text.StartsWith("0")) // handler.text should return 0 if everything went correctly.
            {
                Debug.Log(handler.text);
            }
            else
            {
                Debug.Log($"Posting data failed. Error #{handler.text}");
            }
        }
    }
}
