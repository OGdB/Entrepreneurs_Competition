using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Singleton;

    public NetworkList<PlayerData> Group1;
    public List<PlayerDataSerialized> group1 = new();

    public NetworkList<PlayerData> Group2;
    public List<PlayerDataSerialized> group2 = new();

    private void Awake()
    {
        MakeSingleton();
        CreateGroups();

        void MakeSingleton()
        {
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
        }

        void CreateGroups()
        {
            Group1 = new NetworkList<PlayerData>(
            readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Server
            );
            Group2 = new NetworkList<PlayerData>(
                readPerm: NetworkVariableReadPermission.Everyone,
                writePerm: NetworkVariableWritePermission.Server
                );
        }
    }

    private void Start()
    {
        DBManager.OnLogin += OnLogin;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); 

        // Make our serialized list representation show the network variables that are already there.
        UpdateSerializedList();
    }

    public void UpdateSerializedList()
    {
        // Group 1
        group1.Clear();
        string prettyString = "Group 1: ";

        for (int i = 0; i < Group1.Count; i++)
        {
            var member = Group1[i];

            PlayerDataSerialized prettySerialized = new(member.playerId, member.name.ToString());
            group1.Add(prettySerialized);

            prettyString += member;

            if (i + 1 <= Group1.Count)
                prettyString+= ", ";
        }


        // Group 2
        group2.Clear();
        prettyString = "Group 2: ";

        for (int i = 0; i < Group2.Count; i++)
        {
            var member = Group2[i];

            PlayerDataSerialized prettySerialized = new(member.playerId, member.name.ToString());
            group2.Add(prettySerialized);

            prettyString += member;

            if (i + 1 <= Group2.Count)
                prettyString += ", ";
        }
    }

    /// <summary>
    /// Let the SERVER put the user in the correct group Networklist, which keeps the groups up for all connected clients.
    /// </summary>
    private void OnLogin(string uname) => OnLoginServerRpc(DBManager.UserName, NetworkManager.LocalClientId, DBManager.GroupInt);

    [ServerRpc(RequireOwnership = false)]  // Only runs on server/host
    private void OnLoginServerRpc(string username, ulong userId, int group)
    {
        PlayerData newPlayer = new(userId, username);

        if (group == 1)
        {
            Group1.Add(newPlayer);
        }
        else if (group == 2)
        {
            Group2.Add(newPlayer);
        }

        UpdateSerializedListsClientRpc();
    }

    [ClientRpc]  // Runs on clients (also host)
    private void UpdateSerializedListsClientRpc() => UpdateSerializedList();
}

#region STRUCTS
[Serializable]
public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong playerId;
    public FixedString32Bytes name;

    public PlayerData(ulong playerId, FixedString32Bytes name)
    {
        this.playerId = playerId;
        this.name = name;
    }

    public bool Equals(PlayerData other)
    {
        return playerId.Equals(other.playerId) && name.Equals(other.name);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerId);
        serializer.SerializeValue(ref name);
    }
}
// Just to see it in the inspector nicely.
[Serializable]
public struct PlayerDataSerialized
{
    public ulong playerId;
    public string name;

    public PlayerDataSerialized(ulong playerId, string name)
    {
        this.playerId = playerId;
        this.name = name;
    }
}
#endregion