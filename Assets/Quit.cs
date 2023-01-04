using Unity.Netcode;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void Shutdown()
    {
        if (NetworkManager.Singleton!= null)
            NetworkManager.Singleton.Shutdown();
    }
}
