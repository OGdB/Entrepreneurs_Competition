using System.Collections;
using UnityEngine;

/// <summary>
/// Class representing and handling a Group's building
/// </summary>
public class Group : MonoBehaviour
{
    public int GroupLevel { get => groupLevel; }

    [SerializeField]
    private int groupLevel = 0;
    public Building GroupBuilding { get => groupBuilding; private set => groupBuilding = value; }
    [SerializeField] private Building groupBuilding;

    private GameObject currentBuilding;


    private void OnEnable()
    {
        GameManager.OnLevelUp += OnLevelUp;
        OnQuizResultsClose.OnQuizResultsClosed += OnLevelUp;  // Level up regardless of how much score increase
    }
    private void OnDisable()
    {
        GameManager.OnLevelUp -= OnLevelUp;
        OnQuizResultsClose.OnQuizResultsClosed -= OnLevelUp;
    }

    private void Start()
    {
        GetCurrentBuilding(GroupLevel);
    }

    private void GetCurrentBuilding(int currentLevel)
    {
        if (currentBuilding)
        {
            Destroy(currentBuilding);
            GroupBuilding = null;
        }

        currentBuilding = Instantiate(GameManager.Singleton.Order.GetBuilding(currentLevel), transform);
        currentBuilding.transform.position = transform.position;
        GroupBuilding = currentBuilding.GetComponent<Building>();
    }

    public void OnLevelUp()
    {
        _ = StartCoroutine(LevelUpAnimation());

        IEnumerator LevelUpAnimation()
        {
            print("Start Level Up Notification / Draw Attention to Building.");

            yield return new WaitForSeconds(2f);

            print("Upgrade the building");

            if (groupLevel < GameManager.Singleton.Order.GetOrderLength() - 1)
            {
                groupLevel++;
                GetCurrentBuilding(groupLevel);
            }
            else
            {
                print("Maximum building level reached!");
            }
        }
    }

    
}
