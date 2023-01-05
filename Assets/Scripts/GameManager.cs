using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    #region Properties
    public static GameManager _Instance;

    [Header("Group fields"), SerializeField]
    private Group thisGroup;

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
    #endregion

    private void Awake()
    {
        // Singleton
        {
            if (_Instance != null && _Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _Instance = this;
            }
        }
    }

    private void Start() => CameraHandler.CenterCameraOnPoint(thisGroup.transform.position);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            thisGroup.OnLevelUp();
            OnLevelUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQuizReceived?.Invoke();
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
    public void IncreaseScore(int amount)
    {
        DBManager.Singleton.Score += amount;
    }

    public void PressedReady()
    {
        DBManager.Singleton.ChangePlayerReadyStatus(DBManager.Singleton.currentUser, true);
    }
    public void ExitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif    
    }
}
