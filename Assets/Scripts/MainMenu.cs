using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI playerDisplay;

    public Button registerButton;
    public Button loginButton;
    public Button playButton;
    //public Button logoutButton;

    private void OnEnable() => SetUI();

    private void SetUI()
    {
        if (DBManager.LoggedIn)
            playerDisplay.SetText($"Player: {DBManager.UserName}");
        else
            playerDisplay.SetText("No user logged in");

        loginButton.gameObject.SetActive(!DBManager.LoggedIn);
        registerButton.gameObject.SetActive(!DBManager.LoggedIn);
        playButton.interactable = DBManager.LoggedIn;
        //logoutButton.interactable = DBManager.LoggedIn;
    }

    public void GoToRegisterScene() => SceneManager.LoadScene("Register Menu");
    public void GoToLoginScene() => SceneManager.LoadScene("Login Menu");
    public void GoToGameScene() => SceneManager.LoadScene("City");
    public void GoToLeaderboard() => SceneManager.LoadScene("Leaderboard");

    public void LogOut()
    {
        if (DBManager.LoggedIn)
            DBManager.LogOut();

        SetUI();
    }

}
