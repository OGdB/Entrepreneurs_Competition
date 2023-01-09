using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public TMPro.TMP_InputField chatInput;
    public GameObject messagePrefab;
    public Transform messageParent;
    public Transform contentTransform;

    private void OnEnable()
    {
        chatInput.onEndEdit.AddListener(SendChat);
    }
    private void OnDisable()
    {
        chatInput.onEndEdit.AddListener(SendChat);
    }

    private void SendChat(string message)
    {
        GameObject obj = Instantiate(messagePrefab.gameObject, messageParent);
        TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();

        string user = "";
        if (DBManager.Singleton != null)
        {
            user = $"{DBManager.Singleton.currentUser.Name}: ";
        }

        text.SetText(user + message);
    }
}
