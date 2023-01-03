using UnityEngine;

public class GroupScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject groupOverviewPlayerPrefab;

    private void OnEnable()
    {
        UpdateGroupScreen();
        print(DBManager.GroupInt);
    }

    private void UpdateGroupScreen()
    {
        int playerGroup = DBManager.GroupInt;

        InstantiateGroupOverviewPlayerPrefab(DBManager.UserName);

        if (playerGroup == 1)
        {
            AddMembersToOverview(Players.Singleton.Group1);
        }
        else if (playerGroup == 2)
        {
            AddMembersToOverview(Players.Singleton.Group2);
        }

        void AddMembersToOverview(Unity.Netcode.NetworkList<Unity.Collections.FixedString32Bytes> group)
        {
            for (int i = 0; i < group.Count; i++)
            {
                string playerName = group[i].ToString();

                if (playerName == DBManager.UserName) continue;

                InstantiateGroupOverviewPlayerPrefab(playerName);
            }
        }
    }

    private void InstantiateGroupOverviewPlayerPrefab(string username)
    {
        GameObject thisPlayer = Instantiate(groupOverviewPlayerPrefab, transform.GetChild(0));
        TMPro.TextMeshProUGUI nameText = thisPlayer.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        nameText.SetText(username);
    }
}
