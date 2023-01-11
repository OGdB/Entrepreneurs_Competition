using UnityEngine;

public class BuildingPart : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            GetComponentInParent<Outline>().RecalculateBounds();
        }
    }
    private void OnMouseEnter()
    {
        GetComponentInParent<Outline>().enabled = true;
        GetComponentInParent<CompanyBuilding>().ShowBuildingInfo();
    }
    private void OnMouseExit()
    {
        GetComponentInParent<Outline>().enabled = false;
        GetComponentInParent<CompanyBuilding>().HideBuildingInfo();
    }
}
