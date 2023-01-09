using Unity.VisualScripting;
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
        print("Show UI!");
    }
    private void OnMouseExit()
    {
        GetComponentInParent<Outline>().enabled = false;
        print("Hide UI!");
    }
}
