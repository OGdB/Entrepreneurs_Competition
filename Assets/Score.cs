using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private string preScoreText = "Score: ";
    private void Awake()
    {
        GetComponent<TMPro.TextMeshProUGUI>().SetText(preScoreText + DBManager.Singleton.Score.ToString());
    }
}
