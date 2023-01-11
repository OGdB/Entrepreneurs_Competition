using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    #region Properties
    public static GameManager Singleton;

    [Header("Group fields"), SerializeField]
    private CompanyBuilding playerBuilding;

    public BuildingUpgradeOrder Order { get => order; }

    [SerializeField]
    private BuildingUpgradeOrder order;

    // Events
        // Quiz
    public delegate void QuizReceived();
    public static QuizReceived OnQuizReceived { get => onQuizReceived; set => onQuizReceived = value; }
    private static QuizReceived onQuizReceived;
    // Level up
    public delegate void LevelUp();
    public static LevelUp OnLevelUp { get => onLevelUp; set => onLevelUp = value; }
    private static LevelUp onLevelUp;

    public bool QuizAvailable { get => quizAvailable; set => quizAvailable = value; }
    private bool quizAvailable = false;
    #endregion

    private void Awake()
    {
        // Singleton
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        if (Singleton == this)
            Singleton = null;
    }
    private void Start() => CityCameraHandler.CenterCameraOnPoint(playerBuilding.transform.position);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerBuilding.OnLevelUp();
            OnLevelUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TriggerQuiz();
        }
    }


    /// <summary>
    /// Save the score of this player
    /// </summary>
    public void CallSaveData()
    {
        if (DBManager.Singleton.GroupName == default)
        {
            Debug.LogError("This user is not in a group or its group is not set in the DBManager!");
            return;
        }

        _ = StartCoroutine(SavePlayerData());

        IEnumerator SavePlayerData()
        {
            string scoreString = DBManager.Singleton.Score.ToString();

            List<IMultipartFormSection> formData = new()
            {
                new MultipartFormDataSection(name: "groupname", data: DBManager.Singleton.GroupName),
                new MultipartFormDataSection(name: "score", data: scoreString)
            };

            using var request = UnityWebRequest.Post(DBManager.phpFolderURL + "savedata.php", formData);

            yield return request.SendWebRequest();

            if (request.downloadHandler.text.StartsWith("0"))
            {
                Debug.Log("Game saved!");
            }
            else
            {
                Debug.Log($"Save failed, Error #{request.downloadHandler.text}");
            }
        }
    }

    /// <summary>
    /// Locally increase the score
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseScore(int amount) => DBManager.Singleton.IncreaseScore(amount);

    public void TriggerQuiz()
    {
        QuizAvailable = true;
        OnQuizReceived?.Invoke();
    }

    public void PressedReady() => 
        DBManager.Singleton.ChangePlayerReadyStatus(DBManager.Singleton.currentUser, true);

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif    
    }
}
