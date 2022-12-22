using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GoToScene : MonoBehaviour
{
    public int sceneIndex = 0;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(sceneIndex));
    }
}
