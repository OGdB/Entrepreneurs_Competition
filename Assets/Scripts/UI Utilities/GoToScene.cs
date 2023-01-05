using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Adds a listener to a button to go to a specific scene.
/// </summary>
[RequireComponent(typeof(Button))]
public class GoToScene : MonoBehaviour
{
    public int sceneIndex = 0;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(sceneIndex));
    }
}
