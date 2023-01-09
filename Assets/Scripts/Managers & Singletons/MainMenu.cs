using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Space(5), Tooltip("Log in with test accounts")]
    public bool autoLogin = false;
    public bool instantlyToQuiz = false;
    public bool instantlyToCity = false;
    private static bool loggedIn = false;

    [Space(10), Header("Assignables")]
    public TMPro.TextMeshProUGUI playerDisplay;
    public Button registerButton;
    public Button loginButton;
    public Button playButton;

    private IEnumerator Start()
    {
        if (autoLogin && !loggedIn)
        {
            yield return StartCoroutine(LoginManager.LoginCR("111111111111", "111111111111"));
            yield return StartCoroutine(LoginManager.LoginCR("222222222222", "222222222222"));

            SetUI();

            loggedIn = true;

            if (instantlyToQuiz)
            {
                SceneManager.LoadScene("Quiz");
            }
            if (instantlyToCity)
            {
                SceneManager.LoadScene("City");
            }
        }
    }

    private void OnEnable() => SetUI();

    private void SetUI()
    {
        if (DBManager.LoggedIn())
        {
            string loggedInUsers = "";
            for (int n = 0; n < DBManager.Singleton.Users.Count; n++)
            {
                User member = DBManager.Singleton.Users[n];
                loggedInUsers += member.Name;

                if (DBManager.Singleton.Users.Count > n + 1)
                    loggedInUsers += ", ";
            }

            playerDisplay.SetText($"Player: {loggedInUsers}");
        }
        else
            playerDisplay.SetText("No user logged in");

/*        loginButton.interactable = !DBManager.LoggedIn;
        registerButton.interactable = !DBManager.LoggedIn;*/
        playButton.interactable = DBManager.LoggedIn();
    }

    public void GoToRegisterScene() => SceneManager.LoadScene("Register Menu");
    public void GoToLoginScene() => SceneManager.LoadScene("Login Menu");
    public void GoToGameScene() => SceneTransition.TransitionToScene("City");
    public void GoToLeaderboard() => SceneManager.LoadScene("Leaderboard");

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
