using System.Collections;
using UnityEngine;

namespace Unity.Netcode.Samples
{
    /// <summary>
    /// Class to display helper buttons and status labels on the GUI, as well as buttons to start host/client/server.
    /// Once a connection has been established to the server, the local player can be teleported to random positions via a GUI button.
    /// </summary>
    public class BootstrapManager : MonoBehaviour
    {
        public bool autoStart = false;
        public float autoStartTime = 5f;

        private NetworkManager networkManager;
        private int secretButtonClickCount = 0;

        private IEnumerator Start()
        {
            networkManager = NetworkManager.Singleton;

#if UNITY_EDITOR
            networkManager.StartHost();
            yield break;
#endif
            yield return new WaitForSeconds(autoStartTime);

            if (!networkManager.IsHost && autoStart)
            {
                networkManager.StartClient();
                print("Started as client");
            }
        }

        public void SecretButtonClicked()
        {
            secretButtonClickCount++;
            if (secretButtonClickCount == 5 && !networkManager.IsHost)
            {
                networkManager.StartHost();
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (!networkManager.IsClient && !networkManager.IsServer)
            {
/*                if (GUILayout.Button("Host"))
                {
                    networkManager.StartHost();
                }*/

                if (GUILayout.Button("Client"))
                {
                    networkManager.StartClient();
                }

/*                if (GUILayout.Button("Server"))
                {
                    networkManager.StartServer();
                }*/
            }
            else
            {
                if (networkManager.IsHost)
                    GUILayout.Label("Mode: Host");

                /*// "Random Teleport" button will only be shown to clients
                if (networkManager.IsClient)
                {
                    if (GUILayout.Button("Random Teleport"))
                    {
                        if (networkManager.LocalClient != null)
                        {
                            // Get `BootstrapPlayer` component from the player's `PlayerObject`
                            if (networkManager.LocalClient.PlayerObject.TryGetComponent(out BootstrapPlayer bootstrapPlayer))
                            {
                                // Invoke a `ServerRpc` from client-side to teleport player to a random position on the server-side
                                bootstrapPlayer.RandomTeleportServerRpc();
                            }
                        }
                    }
                }*/
            }

            GUILayout.EndArea();
        }
    }
}
