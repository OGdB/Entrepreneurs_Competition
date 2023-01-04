using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Netcode.Samples
{
    /// <summary>
    /// Class to display helper buttons and status labels on the GUI, as well as buttons to start host/client/server.
    /// Once a connection has been established to the server, the local player can be teleported to random positions via a GUI button.
    /// </summary>
    public class BootstrapManager : MonoBehaviour
    {
        public static BootstrapManager Singleton;

        public bool autoClient = true;
        public bool autoServer = false;
        public float autoStartTime = 5f;

        private NetworkManager networkManager;

        private void Start()
        {
            networkManager = NetworkManager.Singleton;

            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer) return;

#if UNITY_EDITOR
            if (autoClient)
            {
                networkManager.StartHost();
                SceneManager.LoadScene(1);
                return;
            }
            if (autoServer)
            {
                networkManager.StartServer();
                SceneManager.LoadScene(1);
                return;
            }
#endif
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (!networkManager.IsClient && !networkManager.IsServer)
            {
                if (GUILayout.Button("Host"))
                {
                    networkManager.StartHost();
                    SceneManager.LoadScene(1);
                }

                if (GUILayout.Button("Client"))
                {
                    networkManager.StartClient();
                    SceneManager.LoadScene(1);
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
