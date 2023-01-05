using UnityEngine;

/// <summary>
/// Put on a TMPro.UGUI element to make it show the user who's turn it is.
/// </summary>
public class CurrentUser : MonoBehaviour
{
    private void Awake() => SetToCurrentUser();
    private void OnEnable() => DBManager.OnCurrentUserChanged += SetToCurrentUser;
    private void OnDisable() => DBManager.OnCurrentUserChanged -= SetToCurrentUser;
    private void SetToCurrentUser() => 
        GetComponent<TMPro.TextMeshProUGUI>().SetText(DBManager.Singleton.currentUser.Name);
}
