using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DBManager : MonoBehaviour
{
    public static DBManager Singleton;
    public const string phpFolderURL = "https://oeds.online/php/";

    public List<User> Users = new();
    public User currentUser;
    public int currentUserInt = 0;
    public string ClassNumber = null;
    public string GroupName = null;
    public int Score = -1;

    public delegate void CurrentUserChanged();
    public static CurrentUserChanged OnCurrentUserChanged;

    // Logged in if there is an username.
    public static bool LoggedIn { get { return Singleton.Users.Count > 0; } }

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

    public void ChangePlayerReadyStatus(User user, bool newState)
    {
        user.IsReady = newState;
/*        foreach (User u in Users.Where(u => u == user))
        {
            u.IsReady = newState;
        }*/
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