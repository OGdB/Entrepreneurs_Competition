using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ZeroScale: MonoBehaviour
{
    public void ScaleToZero()
    {
        GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
