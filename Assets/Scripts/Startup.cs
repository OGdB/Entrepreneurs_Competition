using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    private void Awake()
    {
        // Loads the Main Menu scene without unloading anything from this scene.
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
    }
}
