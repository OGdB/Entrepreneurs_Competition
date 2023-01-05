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

    private void OnEnable()
    {
        UpdateGroup();
        groupNameText.SetText(DBManager.Singleton.GroupName);

        DBManager.OnPressedReady += OnPlayerPressedReady;

    }
    private void OnDestroy()
    {
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
            string ready = member.IsReady ? "Ready" : "Not Ready";
            
            GameObject go = Instantiate(overviewPlayerGo, parent);
            OverviewPlayerObject opo = new(go, member);
            overviewPlayerGos.Add(opo);
            go.name = name;

            TMPro.TextMeshProUGUI nameText = go.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            nameText.SetText(name);
            TMPro.TextMeshProUGUI readyText = go.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            readyText.SetText(ready);
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
