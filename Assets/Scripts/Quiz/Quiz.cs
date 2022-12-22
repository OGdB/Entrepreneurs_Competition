using UnityEngine;
using TMPro;

public class Quiz : MonoBehaviour
{
    [SerializeField]
    private Animation QuizPopupAnimation;
    [SerializeField]
    private TextMeshProUGUI quizIntroText;

    private void OnEnable()
    {
        GameManager.OnQuizReceived += OnQuizReceived;
    }
    private void OnDisable()
    {
        GameManager.OnQuizReceived -= OnQuizReceived;
    }

    public void PlayQuickPopUpAnimation() => QuizPopupAnimation.Play();

    public void OnQuizReceived()
    {
        PlayQuickPopUpAnimation();
    }

}
