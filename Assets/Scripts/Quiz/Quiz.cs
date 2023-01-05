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

        DBManager.OnPressedReady += OnReadyWasPressed;
    }
    private void OnDisable()
    {
        GameManager.OnQuizReceived -= OnQuizReceived;

        DBManager.OnPressedReady -= OnReadyWasPressed;
    }

    public void PlayQuickPopUpAnimation() => QuizPopupAnimation.Play();

    public void OnQuizReceived() => PlayQuickPopUpAnimation();

    public void PressedLater() => gameObject.SetActive(false);

    private void OnReadyWasPressed(bool allReady)
    {
        if (allReady)
            gameObject.SetActive(false);
    }
}
