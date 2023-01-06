using System.Collections.Generic;
using UnityEngine;

public class GroupScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject overviewPlayerGo;
    private List<OverviewPlayerObject> overviewPlayerGos = new();
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private GameObject readyButton;
    [SerializeField]
    private TMPro.TextMeshProUGUI groupNameText;
    [SerializeField]
    private Color notReadyColor = Color.white;
    [SerializeField]
    private Color readyColor = Color.white;

    private void OnEnable()
    {
        UpdateGroup();
        groupNameText.SetText(DBManager.Singleton.GroupName);

        // When a quiz comes in, update group screen so it shows everyone's ready status.
        GameManager.OnQuizReceived += UpdateGroup;
        DBManager.OnPressedReady += OnPlayerPressedReady;
    }
    private void OnDisable()
    {
        GameManager.OnQuizReceived -= UpdateGroup;
        DBManager.OnPressedReady -= OnPlayerPressedReady;
    }

    private void UpdateGroup()
    {
        foreach (var opo in overviewPlayerGos)
        {
            Destroy(opo.overviewPlayerGo);
        }

        overviewPlayerGos.Clear();

        foreach (var member in DBManager.Singleton.Users)
        {
            string name = member.Name;

            GameObject go = Instantiate(overviewPlayerGo, parent);
            OverviewPlayerObject opo = new(go, member);
            overviewPlayerGos.Add(opo);
            go.name = name;

            TMPro.TextMeshProUGUI nameText = go.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            nameText.SetText(name);

            // Only show ready status if there's a quiz available
            TMPro.TextMeshProUGUI readyText = go.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
            if (GameManager.Singleton.QuizAvailable)
            {
                readyText.gameObject.SetActive(true);

                if (member.IsReady)
                {
                    readyText.color = readyColor;
                    readyText.SetText("Ready");
                }
                else
                {
                    readyText.color = notReadyColor;
                    readyText.SetText("Not Ready");
                }
            }
            else
            {
                readyText.gameObject.SetActive(false);
            }
        }
    }

    public void OnPlayerPressedReady(bool allReady)
    {
        UpdateGroup();

        if (allReady)
            readyButton.SetActive(false);
    }
}

[System.Serializable]
public class OverviewPlayerObject
{
    public GameObject overviewPlayerGo;
    public User user;

    public OverviewPlayerObject(GameObject overviewPlayerGo, User user)
    {
        this.overviewPlayerGo = overviewPlayerGo;
        this.user = user;
    }
}
