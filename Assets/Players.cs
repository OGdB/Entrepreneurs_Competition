using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

public class Players : NetworkBehaviour
{
    public static Players Singleton;

    public NetworkList<FixedString32Bytes> Group1 { get => group1; private set => group1 = value; }
    private NetworkList<FixedString32Bytes> group1;
    public List<string> testGroup1List = new();

    public NetworkList<FixedString32Bytes> Group2 { get => group2; private set => group2 = value; }
    private NetworkList<FixedString32Bytes> group2;
    public List<string> testGroup2List = new();

    public static int playerGroup = 0;  // The group this player belongs to.


    private void Awake()
    {
        if (Singleton)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        Group1 = new NetworkList<FixedString32Bytes>();
        Group2 = new NetworkList<FixedString32Bytes>();
    }

    private void OnEnable()
    {
        DBManager.OnLogin += OnLogin;

        UpdateTestList();
    }
    private void OnDisable()
    {
        DBManager.OnLogin -= OnLogin;
    }

    private void UpdateTestList()
    {
        // Group 1
        testGroup1List.Clear();
        string prettyString = "Group 1: ";

        for (int i = 0; i < Group1.Count; i++)
        {
            var id = Group1[i];

            testGroup1List.Add(id.ToString());
            prettyString += id;

            if (i+1 <= Group1.Count)
                prettyString+= ", ";
        }


        // Group 2
        testGroup2List.Clear();
        prettyString = "Group 2: ";

        for (int i = 0; i < Group2.Count; i++)
        {
            var id = Group2[i];

            testGroup2List.Add(id.ToString());
            prettyString += id;

            if (i + 1 <= Group2.Count)
                prettyString += ", ";
        }
    }

    private void OnLogin()
    {
        OnLoginServerRpc(DBManager.UserName, DBManager.GroupName);
    }
    [ServerRpc(RequireOwnership = false)]
    private void OnLoginServerRpc(string username, string group, ServerRpcParams serverParams = default)
    {
        if (group == "Group1")
        {
            Group1.Add(username);
            playerGroup = 1;

        }
        else if (group == "Group2")
        {
            Group2.Add(username);
            playerGroup = 2;
        }

        UpdateTestList();
    }
}