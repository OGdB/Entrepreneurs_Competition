using UnityEngine;
using UnityEngine.SceneManagement;

public class OnQuizResultsClose : MonoBehaviour
{
    public delegate void QuizResultsClosed();
    public static QuizResultsClosed OnQuizResultsClosed;

    public void CloseQuizResults()
    {
        SceneManager.UnloadSceneAsync("Quiz");
        OnQuizResultsClosed?.Invoke();
    }
}
