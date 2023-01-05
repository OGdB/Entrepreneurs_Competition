using UnityEngine;

public class CurrentUser : MonoBehaviour
{
    private void Awake()
    {
        SetToCurrentUser();
    }
    private void OnEnable() => DBManager.OnCurrentUserChanged += SetToCurrentUser;
    private void OnDisable() => DBManager.OnCurrentUserChanged -= SetToCurrentUser;
    private void SetToCurrentUser() => 
        GetComponent<TMPro.TextMeshProUGUI>().SetText(DBManager.Singleton.currentUser.Name);
}
