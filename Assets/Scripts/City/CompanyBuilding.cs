using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class CompanyBuilding : MonoBehaviour
{
    [SerializeField]
    private Transform textTransform;
    private Vector3 textStartPos;
    public int GroupLevel { get => groupLevel; }

    [SerializeField]
    private int groupLevel = 0;

    private GameObject currentBuilding;
    public bool BuildingSpawned { get => buildingSpawned; private set => buildingSpawned = value; }
    private bool buildingSpawned = false;

    public bool isPlayerBuilding = false;


    private void Awake() => textStartPos = textTransform.position;
    private void Start()
    {
        GetCurrentBuilding(GroupLevel);

        GetComponent<Outline>().RecalculateBounds();
        SetTextPosition();

        if (isPlayerBuilding)
            textTransform.GetComponent<TMPro.TextMeshPro>().SetText(DBManager.Singleton.GroupName);
        else
        {
            textTransform.GetComponent<TMPro.TextMeshPro>().SetText(DBManager.Singleton.otherRandomGroupName);
        }
    }
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

    private void GetCurrentBuilding(int currentLevel)
    {
        buildingSpawned = false;
        if (currentBuilding)
        {
            Destroy(currentBuilding);
        }

        currentBuilding = Instantiate(GameManager.Singleton.Order.GetBuilding(currentLevel), transform);
        //currentBuilding.transform.localPosition = Vector3.zero;
        buildingSpawned = true;
    }

    private void SetTextPosition()
    {
        MeshRenderer[] renderer = transform.GetComponentsInChildren<MeshRenderer>();
        float totalBuildingSize = 0f;
        for (int i = 0; i < renderer.Length; i++)
        {
            MeshRenderer buildingPart = renderer[i];
            totalBuildingSize += buildingPart.bounds.size.y;
        }

        Vector3 textPos = textStartPos;
        textPos.y += totalBuildingSize;
        textTransform.transform.position = textPos;
        textTransform.GetComponent<Hover>().basePosition = textPos;
    }

    public void AccentuateBuilding() => Accentuate.AccentuateObject(gameObject, 0.5f);
    public void UnaccentuateBuilding() => Accentuate.UnAccentuateObject(gameObject);

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
