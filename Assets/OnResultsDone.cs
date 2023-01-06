using UnityEngine;
using UnityEngine.SceneManagement;

public class OnResultsDone : MonoBehaviour
{
    public void UnloadQuizScene()
    {
        SceneManager.UnloadSceneAsync("Quiz");
    }
}
