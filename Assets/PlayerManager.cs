using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Singleton;

    public NetworkList<FixedString32Bytes> Group1;
    public List<string> group1 = new();

    public NetworkList<FixedString32Bytes> Group2;
    public List<string> group2 = new();

    private void Awake()
    {
        Group1 = new NetworkList<FixedString32Bytes>(
readPerm: NetworkVariableReadPermission.Everyone,
writePerm: NetworkVariableWritePermission.Server
);
        Group2 = new NetworkList<FixedString32Bytes>(
            readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Server
            );

        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        //GetComponent<NetworkObject>().Spawn();

        DBManager.OnLogin += OnLogin;

    }

    private void UpdateSerializedList()
    {
        // Group 1
        group1.Clear();
        string prettyString = "Group 1: ";

        for (int i = 0; i < Group1.Count; i++)
        {
            print("Add to group 1");

            var id = Group1[i];

            group1.Add(id.ToString());
            prettyString += id;

            if (i + 1 <= Group1.Count)
                prettyString+= ", ";
        }


        // Group 2
        group2.Clear();
        prettyString = "Group 2: ";

        for (int i = 0; i < Group2.Count; i++)
        {
            var id = Group2[i];

            group2.Add(id.ToString());
            prettyString += id;

            if (i + 1 <= Group2.Count)
                prettyString += ", ";
        }
    }
/*
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        DBManager.OnLogin += OnLogin;
    }
*/
    /// <summary>
    /// Let the SERVER put the user in the correct group Networklist, which keeps the groups up for all connected clients.
    /// </summary>
    private void OnLogin(string uname)
    {
        OnLoginServerRpc(uname, DBManager.GroupInt);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnLoginServerRpc(string username, int group)
    {
        if (group == 1)
        {
            Group1.Add((FixedString32Bytes)username);
        }
        else if (group == 2)
        {
            Group2.Add((FixedString32Bytes)username);
        }

        UpdateSerializedList();

        UpdateSerializedListsClientRpc();
    }

    [ClientRpc]
    private void UpdateSerializedListsClientRpc()
    {
        UpdateSerializedList();
    }
}