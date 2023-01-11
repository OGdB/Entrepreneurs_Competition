using UnityEngine;

public class GroupName : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().SetText(DBManager.Singleton.GroupName);
    }
}
