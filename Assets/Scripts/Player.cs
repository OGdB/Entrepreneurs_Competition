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
    
}
