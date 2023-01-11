using UnityEngine;

public class BuildingInfoPopUp : MonoBehaviour
{
    private static BuildingInfoPopUp Singleton;
    [SerializeField]
    private TMPro.TextMeshProUGUI popupText;

    public static void SetPopUpText(string text)
    {
        Singleton.gameObject.SetActive(true);
        Singleton.popupText.text = text;
    }
    public static void HidePopUpText()
    {
        Singleton.gameObject.SetActive(false);
        Singleton.popupText.text = "";
    }
    private void Awake()
    {
        Singleton = this;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Singleton = null;
    }
}
