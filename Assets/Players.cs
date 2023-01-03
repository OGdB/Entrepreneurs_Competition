using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

public class Players : NetworkBehaviour
{
    public static Players instance;

    public TMPro.TextMeshProUGUI testText;

    private NetworkList<FixedString32Bytes> group1;
    public List<string> testGroup1List = new();

    private void Awake()
    {
        group1 = new NetworkList<FixedString32Bytes>();

        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public override void OnNetworkSpawn()
    {
        SendClientMessageServerRpc();
    }

    // Run only on server (once)
    [ServerRpc(RequireOwnership = false)]
    public void SendClientMessageServerRpc(ServerRpcParams serverParams = default)
    {
        int senderClientId = (int)serverParams.Receive.SenderClientId;
        group1.Add((FixedString32Bytes)senderClientId.ToString());
        
        testGroup1List.Clear();
        string finalMessage = "Players: ";
        foreach (var clientId in group1)
        {
            print(clientId);
            testGroup1List.Add(clientId.ToString());
            finalMessage += $"{clientId}, ";
        }

        SendChatMessageClientRpc(finalMessage);
    }

    // Run on all clients (also runs on host)
    [ClientRpc]
    public void SendChatMessageClientRpc(string message)
    {
        testText.SetText(message);
    }
}
