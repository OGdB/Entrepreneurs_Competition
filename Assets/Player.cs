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
        id = (int)NetworkManager.LocalClientId;

        if (IsLocalPlayer)
            DBManager.OnLogin += OnLogin;
    }

    private void OnLogin()
    {
        username = DBManager.UserName;
        name = DBManager.UserName;
    }
    
}
