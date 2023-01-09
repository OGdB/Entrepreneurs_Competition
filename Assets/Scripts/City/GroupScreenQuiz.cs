using System.Collections.Generic;
using UnityEngine;

public class GroupScreenQuiz : MonoBehaviour
{
    [SerializeField]
    private GameObject overviewPlayerGo;
    private List<OverviewPlayerObject> overviewPlayerGos = new();

    private void OnEnable()
    {
        UpdateGroup();
        DBManager.OnCurrentUserChanged += UpdateGroup;
    }
    private void OnDisable()
    {
        DBManager.OnCurrentUserChanged -= UpdateGroup;
    }

    private void UpdateGroup()
    {
        foreach (var opo in overviewPlayerGos)
        {
            Destroy(opo.overviewPlayerGo);
        }

        overviewPlayerGos.Clear();

        for (int u = 0; u < DBManager.Singleton.Users.Count; u++)
        {
            User member = DBManager.Singleton.Users[u];
            string name = member.Name;

            GameObject go = Instantiate(overviewPlayerGo, transform);
            OverviewPlayerObject opo = new(go, member);
            overviewPlayerGos.Add(opo);
            go.name = name;

            TMPro.TextMeshProUGUI nameText = go.GetComponent<TMPro.TextMeshProUGUI>();

            if (name == DBManager.Singleton.currentUser.Name)
            {
                name = $"<u>{name}<u>";
                go.transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                go.transform.localScale = Vector3.one;
            }

            nameText.SetText(name);
        }
    }
}
