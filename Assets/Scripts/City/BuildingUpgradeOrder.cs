using UnityEngine;

[CreateAssetMenu(fileName = "Building Order", menuName = "ScriptableObjects/BuildingOrder", order = 1)]
public class BuildingUpgradeOrder : ScriptableObject
{
    [SerializeField]
    private GameObject[] buildingOrder;
    public int GetOrderLength() => buildingOrder.Length;

    public GameObject GetBuilding(int index)
    {
        if (index <= buildingOrder.Length)
            return buildingOrder[index];
        else
        {
            Debug.LogWarning("Requested building with index outside of range of building order!");
            return null;
        }
    }
}