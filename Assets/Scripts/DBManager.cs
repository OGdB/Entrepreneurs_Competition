public static class DBManager
{
    public const string phpFolderURL = "https://oeds.online/php/";

    public static string UserName;
    public static string ClassNumber;
    public static string GroupName;
    public static int Score = -1;

    // Logged in if there is an username.
    public static bool LoggedIn { get { return UserName != null; } }

    public static void LogIn(string name, string classNumber, int score = 0, string groupName = default)
    {
        UserName = name;
        ClassNumber = classNumber;
        GroupName = groupName;
        Score = score;
    }

    public static void LogOut()
    {
        UserName = null;
        ClassNumber = null;
        Score = -1;
    }
}
