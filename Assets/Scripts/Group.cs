using UnityEngine;

public class Group : MonoBehaviour
{
    private int groupNumber;
    public int GroupLevel { get => groupLevel; }
    [SerializeField]
    private int groupLevel = 0;
    [SerializeField]
    private Building groupBuilding;

    private GameObject currentBuilding;


    private void OnEnable()
    {
        GameManager.OnLevelUp += OnLevelUp;
    }
    private void OnDisable()
    {
        GameManager.OnLevelUp -= OnLevelUp;
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
            groupBuilding = null;
        }

        currentBuilding = Instantiate(GameManager._Instance.Order.GetBuilding(currentLevel), transform);
        currentBuilding.transform.position = transform.position;
        groupBuilding = currentBuilding.GetComponent<Building>();
    }

    public void OnLevelUp()
    {
        if (groupLevel < GameManager._Instance.Order.GetOrderLength() - 1)
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
