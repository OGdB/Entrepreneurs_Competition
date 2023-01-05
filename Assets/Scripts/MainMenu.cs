using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI playerDisplay;

    public Button registerButton;
    public Button loginButton;
    public Button playButton;
    public Button logoutButton;

    private void OnEnable()
    {
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

            playerDisplay.SetText($"Player: {loggedInUsers}");
        }
        else
            playerDisplay.SetText("No user logged in");

/*        loginButton.interactable = !DBManager.LoggedIn;
        registerButton.interactable = !DBManager.LoggedIn;*/
        playButton.interactable = DBManager.LoggedIn();
        logoutButton.interactable = DBManager.LoggedIn();
    }

    public void GoToRegisterScene()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToLoginScene()
    {
        SceneManager.LoadScene(2);
    }
    public void GoToGameScene()
    {
        SceneManager.LoadScene(3);
    }
    public void GoToLeaderboard()
    {
        SceneManager.LoadScene(4);
    }
}
