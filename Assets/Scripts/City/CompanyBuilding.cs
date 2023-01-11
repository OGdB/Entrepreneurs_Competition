using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class CompanyBuilding : MonoBehaviour
{
    [SerializeField]
    private Transform textTransform;
    public int GroupLevel { get => groupLevel; }

    [SerializeField]
    private int groupLevel = 0;

    private GameObject currentBuilding;
    public bool BuildingSpawned { get => buildingSpawned; private set => buildingSpawned = value; }
    private bool buildingSpawned = false;

    public bool isPlayerBuilding = false;


    private void Start()
    {
        GetCurrentBuilding(GroupLevel);
        GetComponentInChildren<Outline>().RecalculateBounds();
        SetTextPosition();

        if (isPlayerBuilding)
            textTransform.GetComponent<TMPro.TextMeshPro>().SetText(DBManager.Singleton.GroupName);
        else
            textTransform.GetComponent<TMPro.TextMeshPro>().SetText(DBManager.Singleton.otherRandomGroupName);
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
        GetComponentInChildren<Outline>().RecalculateBounds();

        if (isPlayerBuilding)
            GetComponentInChildren<Outline>().OutlineColor=  Color.green;


        buildingSpawned = true;
    }

    private void SetTextPosition()
    {
        _ = StartCoroutine(SetTextDelay());
        IEnumerator SetTextDelay()
        {
            yield return new WaitForFixedUpdate();
            Renderer[] renderer = transform.GetComponentsInChildren<Renderer>();
            float totalBuildingSize = 0f;
            for (int i = 0; i < renderer.Length; i++)
            {
                Renderer buildingPart = renderer[i];
                totalBuildingSize += buildingPart.bounds.size.y;
            }

            Vector3 textPos = new(transform.localPosition.x, totalBuildingSize, transform.localPosition.z);
            textTransform.transform.localPosition = textPos;
            textTransform.GetComponent<Hover>().basePosition = textPos;
        }
    }

    public void AccentuateBuilding() => Accentuate.AccentuateObject(gameObject, 0.6f);

    public void UnaccentuateBuilding() => Accentuate.UnAccentuateObject(gameObject);

    public void OnLevelUp()
    {
        _ = StartCoroutine(LevelUpAnimation());

        IEnumerator LevelUpAnimation()
        {
            if (isPlayerBuilding)
            {
                //print("Start Level Up Notification / Draw Attention to Building.");

                yield return new WaitForSeconds(2f);

                //print("Upgrade the building");

                if (groupLevel < GameManager.Singleton.Order.GetOrderLength() - 1)
                {
                    groupLevel++;
                    GetCurrentBuilding(groupLevel);
                    GetComponent<AudioSource>().Play();
                    GetComponentInChildren<Outline>().enabled = true;
                    SetTextPosition();
                }
                else
                {
                    //print("Maximum building level reached!");
                }
            }
        }
    }

    /// <summary>
    /// Show info on this business
    /// </summary>
    internal void ShowBuildingInfo()
    {
        if (isPlayerBuilding)
        {
            BuildingInfoPopUp.SetPopUpText($"{DBManager.Singleton.GroupName}\n" +
                $"Net Worth: {string.Format("{0:C}", DBManager.Singleton.Score)}\n" +
                $"Business Level {DBManager.Singleton.Level}");
        }
        else
        {
            BuildingInfoPopUp.SetPopUpText("Apple\n" +
    "Net Worth: $1,745,000.000\n" +
    "Business Level 7");
        }
    }
    /// <summary>
    /// Hide info on this business
    /// </summary>
    internal void HideBuildingInfo()
    {
        BuildingInfoPopUp.HidePopUpText();
    }
}
