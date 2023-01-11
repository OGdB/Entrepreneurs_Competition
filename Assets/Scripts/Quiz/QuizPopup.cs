using UnityEngine;
using TMPro;

public class QuizPopup : MonoBehaviour
{
    [SerializeField]
    private Animation QuizPopupAnimation;
    [SerializeField]
    private TextMeshProUGUI quizIntroText;

    private void OnEnable()
    {
        quizIntroText.SetText($"{DBManager.Singleton.GroupName} " +
            $"has a meeting with a big investor today!\r\nAre you ready to go?");

        GameManager.OnQuizReceived += OnQuizReceived;

        DBManager.OnPressedReady += OnReadyWasPressed;
    }
    private void OnDisable()
    {
        GameManager.OnQuizReceived -= OnQuizReceived;

        DBManager.OnPressedReady -= OnReadyWasPressed;
    }

    public void PlayQuickPopUpAnimation() => QuizPopupAnimation.Play();

    public void OnQuizReceived()
    {
        GetComponent<AudioSource>().Play();
        PlayQuickPopUpAnimation();
    }

    public void PressedLater() => gameObject.SetActive(false);

    private void OnReadyWasPressed(bool allReady)
    {
        if (allReady)
            gameObject.SetActive(false);
    }
}
