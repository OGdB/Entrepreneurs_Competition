using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public int Id { get => id; set => id = value; }
    [SerializeField]
    private int id;
    public string Name { get => username; set => username = value; }
    [SerializeField]
    private string username;
    public bool IsReady { get => isReady; set => isReady = value; }
    [SerializeField]
    private bool isReady = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        DontDestroyOnLoad(gameObject);
        
        id = (int)OwnerClientId;

        if (IsLocalPlayer)
            DBManager.OnLogin += OnLoginServerRpc;

        NetworkManager.OnClientDisconnectCallback += OnClientDisconnectedServerRpc;
    }
    public override void OnLostOwnership()
    {
        base.OnLostOwnership();
        GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnLoginServerRpc(string username)
    {
        OnLoginClientRpc(username);
        this.username = username;
        name = username;
    }
    [ClientRpc]
    private void OnLoginClientRpc(string uname)
    {
        username = uname;
        name = uname;
    }

    [ServerRpc(RequireOwnership = false)] // Runs only on server/host
    private void OnClientDisconnectedServerRpc(ulong clientId)
    {
        // Remove the player with this ID from their corresponding group.
        for (int i = 0; i < PlayerManager.Singleton.Group1.Count; i++)
        {
            var member = PlayerManager.Singleton.Group1[i];
            if (member.playerId == clientId)
            {
                _ = PlayerManager.Singleton.Group1.Remove(member);
            }
        }
        for (int i = 0; i < PlayerManager.Singleton.Group2.Count; i++)
        {
            var member = PlayerManager.Singleton.Group2[i];
            if (member.playerId == clientId)
            {
                _ = PlayerManager.Singleton.Group2.Remove(member);
            }
        }

        OnClientDisconnectedClientRpc();
    }

    [ClientRpc] // Runs on all clients (also host)
    private void OnClientDisconnectedClientRpc()
    {
        PlayerManager.Singleton.UpdateSerializedList();
    }

}
