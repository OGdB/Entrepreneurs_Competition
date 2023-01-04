public static class DBManager
{
    public const string phpFolderURL = "https://oeds.online/php/";

    public static string UserName;
    public static string ClassNumber;
    public static string GroupName;
    public static int GroupInt = 0;
    public static int Score = -1;

    // Logged in if there is an username.
    public static bool LoggedIn { get { return UserName != null; } }

    public delegate void Login(string username);
    public static Login OnLogin;

    public static void LogIn(string name, string classNumber, int score = 0, string groupName = default)
    {
        UserName = name;
        ClassNumber = classNumber;
        GroupName = groupName;
        Score = score;

        if (groupName == "Group1")
            GroupInt = 1;
        else if (groupName == "Group2")
            GroupInt = 2;


        OnLogin?.Invoke(DBManager.UserName);
    }

    public static void LogOut()
    {
        UserName = null;
        ClassNumber = null;
        Score = -1;
    }
}
