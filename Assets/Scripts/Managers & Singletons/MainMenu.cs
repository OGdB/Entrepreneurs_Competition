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
    public bool instantlyToTestCity = false;
    private static bool loggedIn = false;

    [Space(10), Header("Assignables")]
    public TMPro.TextMeshProUGUI playerDisplay;
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
                SceneTransition.TransitionToScene("City");
            }
            if (instantlyToTestCity)
            {
                SceneManager.LoadScene("City Sandbox 2");
            }
        }
    }

    private void OnEnable()
    {
        DBManager.OnLogin += SetUI;
        SetUI();
    }

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

            playerDisplay.SetText($"Users logged in: {loggedInUsers}");
        }
        else
            playerDisplay.SetText("No user logged in");

        playButton.gameObject.SetActive(DBManager.Singleton.Users.Count > 0);
    }

    public void GoToGameScene() => SceneTransition.TransitionToScene("City");

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
