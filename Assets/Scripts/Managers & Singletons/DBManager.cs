using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Singleton;
    public const string phpFolderURL = "https://oeds.online/php/";

    public static int AmountOfUsers => Singleton.Users.Count;
    public List<User> Users = new();
    public User currentUser;
    public int currentUserInt = 0;
    public string ClassNumber = null;
    public string GroupName = null;
    public int Score = -1;

    public delegate void CurrentUserChanged();
    public static CurrentUserChanged OnCurrentUserChanged;

    public delegate void PressedReady(bool allReady);
    public static PressedReady OnPressedReady;

    // Logged in if there is an username.
    public static bool LoggedIn() => Singleton.Users.Count > 0;
    public static bool LoggedIn(string username)
    {
        foreach (User user in Singleton.Users)
        {
            if (user.Name == username)
                return true;
        }
        return false;
    }
    
    
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogIn(string name, string classNumber, int score, string groupName = default)
    {
        Users.Add(new(name));

        if (Users.Count == 1) // If first user make this one the current.
            currentUser = Users[currentUserInt];

        if (ClassNumber.Length == 0)
            ClassNumber = classNumber;
        if (GroupName.Length == 0)
            GroupName = groupName;

        if (Score == -1)
            Score = score;
    }

    public void NextUser()
    {
        currentUserInt++;
        if (currentUserInt == Users.Count)
        {
            currentUserInt = 0;
        }
        currentUser = Users[currentUserInt];

        OnCurrentUserChanged?.Invoke();
    }

    /// <summary>
    /// Change a single player's ready status and return if all players are now ready.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="newState"></param>
    /// <returns>Are all players ready?</returns>
    public bool ChangePlayerReadyStatus(User user, bool newState)
    {
        user.IsReady = newState;
        NextUser();

        bool allReady = true;
        foreach (var u in Singleton.Users)
        {
            if (!u.IsReady)
            {
                allReady = false;
                break;
            }
        }

        OnPressedReady?.Invoke(allReady);
        return allReady;
    }
}

[System.Serializable]
public class User
{
    public string Name;
    public bool IsReady = false;

    public User(string Name, bool IsReady = false)
    {
        this.Name = Name;
        this.IsReady = IsReady;
    }
}