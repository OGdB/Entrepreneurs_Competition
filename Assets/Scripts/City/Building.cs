using UnityEngine;

[DisallowMultipleComponent]
public class Building : MonoBehaviour
{
    public bool IncreaseYByHalf = false;

    private void Start()
    {
        if (IncreaseYByHalf)
        {
            //Renderer objectRenderer = obj.GetComponentInChildren<Renderer>();
            Collider objectCollider = GetComponentInChildren<Collider>();

            float halfBuildingHeight = objectCollider.bounds.extents.y;
            transform.Translate(Vector3.up * halfBuildingHeight);
        }
    }
}
