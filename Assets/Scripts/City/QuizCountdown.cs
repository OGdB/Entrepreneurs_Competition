using System.Collections;
using UnityEngine;

public class QuizCountdown : MonoBehaviour
{
    [SerializeField] 
    private int countdownLength = 5;
    [SerializeField] 
    private TMPro.TextMeshProUGUI secondsLeftText;

    private int secondsLeft;
    private WaitForSeconds second;
    private Coroutine countdownRoutine;

    [SerializeField]
    private GameObject UI;

    private void Awake()
    {
        UI.SetActive(false);
        second = new(1);
    }
    private void OnEnable() => DBManager.OnPressedReady += OnUsersReady;
    private void OnDisable() => DBManager.OnPressedReady -= OnUsersReady;

    private void OnUsersReady(bool allReady)
    {
        if (allReady && countdownRoutine == null)
        {
            UI.SetActive(true);
            countdownRoutine = StartCoroutine(QuizCountdownCR());
        }
    }

    private IEnumerator QuizCountdownCR()
    {
        secondsLeft = countdownLength;
        secondsLeftText.SetText(secondsLeft.ToString());

        while (secondsLeft > 0)
        {
            yield return second;
            secondsLeft--;
            secondsLeftText.SetText(secondsLeft.ToString());
        }

        SceneTransition.TransitionToScene("Quiz");
    }
}
