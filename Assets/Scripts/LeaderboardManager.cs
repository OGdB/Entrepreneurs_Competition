using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private TextMeshProUGUI rankingOneNumber;
    [SerializeField]
    private TextMeshProUGUI rankingOneName;
    [SerializeField]
    private TextMeshProUGUI rankingOneScore;
    [SerializeField, Space(10)]
    private TextMeshProUGUI rankingTwoNumber;
    [SerializeField]
    private TextMeshProUGUI rankingTwoName;
    [SerializeField]
    private TextMeshProUGUI rankingTwoScore;
    [SerializeField, Space(10)]
    private TextMeshProUGUI rankingThreeNumber;
    [SerializeField]
    private TextMeshProUGUI rankingThreeName;
    [SerializeField]
    private TextMeshProUGUI rankingThreeScore;
    [SerializeField, Space(10)]
    private TextMeshProUGUI rankingCurrentNumber;
    [SerializeField]
    private TextMeshProUGUI rankingCurrentName;
    [SerializeField]
    private TextMeshProUGUI rankingCurrentScore;
    [SerializeField, Space(15)]
    private GameObject NoScoreText;
    #endregion

    private void OnEnable()
    {
        ClearRankings();

        CallLeaderboard();
    }

    public void CallLeaderboard()
    {
        _ = StartCoroutine(LeaderboardCR());

        IEnumerator LeaderboardCR()
        {
            using (var request = UnityWebRequest.Get(DBManager.phpFolderURL + "leaderboard.php"))
            {
                DownloadHandler handler = new DownloadHandlerBuffer();
                request.downloadHandler = handler;

                yield return request.SendWebRequest();

                if (handler.text.StartsWith("0"))
                {
                    string result = handler.text.Trim();

                    // Dissect the result through its tabs.
                    string[] results = result.Split("\t");

                    if (results.Length == 1) // Query succesful, but no top 3 ranking exists yet
                    {
                        ClearRankings();
                        NoScoreText.SetActive(true);
                    }
                    else
                        NoScoreText.SetActive(false);

                    if (results.Length >= 2)
                    {
                        rankingOneNumber.SetText("1.");
                        rankingOneName.SetText(results[1]);
                        rankingOneScore.SetText(results[2]);
                    }

                    if (results.Length >= 4)
                    {
                        rankingTwoNumber.SetText("2.");
                        rankingTwoName.SetText(results[3]);
                        rankingTwoScore.SetText(results[4]);
                    }

                    if (results.Length >= 6)
                    {
                        rankingThreeNumber.SetText("3.");
                        rankingThreeName.SetText(results[5]);
                        rankingThreeScore.SetText(results[6]);
                    }
                }
                else
                {
                    print($"Something went wrong!\nError: {handler.text}");
                }
            }
        }
    }

    private void ClearRankings()
    {
        rankingOneNumber.SetText("");
        rankingOneName.SetText("");
        rankingOneScore.SetText("");

        rankingTwoNumber.SetText("");
        rankingTwoName.SetText("");
        rankingTwoScore.SetText("");

        rankingThreeNumber.SetText("");
        rankingThreeName.SetText("");
        rankingThreeScore.SetText("");

        rankingCurrentNumber.SetText("");
        rankingCurrentName.SetText("");
        rankingCurrentScore.SetText("");
    }
}
