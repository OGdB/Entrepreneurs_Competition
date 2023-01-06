using System.Collections;
using UnityEngine;

public class QuizTimer : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private float totalAmountOfSeconds = 60;
    private float secondsLeft;

    [Space(10), SerializeField]
    private TMPro.TextMeshProUGUI timerText;   
    [SerializeField]
    private TMPro.TextMeshProUGUI resultsScreenTimeLeft;
    [SerializeField]
    private AudioClip tickSound;
    [SerializeField]
    private float tickVolume = 1f;

    private WaitForSeconds second;

    public delegate void TimerUp();
    public static TimerUp OnTimerUp;
    #endregion

    private void Awake() => second = new WaitForSeconds(1);

    private void OnEnable()
    {
        QuizManager.OnQuizStart += OnQuizStart;
        QuizManager.OnQuizOver += OnQuizOver;
        OnTimerUp += StopTimer;
    }
    private void OnDisable()
    {
        QuizManager.OnQuizStart -= OnQuizStart;
        QuizManager.OnQuizOver -= OnQuizOver;
        OnTimerUp -= StopTimer;
    }

    private void OnQuizStart() => StartTimer();
    private void OnQuizOver()
    {
        float secondsInMinute = Mathf.FloorToInt(secondsLeft % 60);
        float minutesLeft = Mathf.FloorToInt(secondsLeft / 60);
        string timeLeft = string.Format("{0:00}:{1:00}", minutesLeft, secondsInMinute);
        resultsScreenTimeLeft.SetText($"Time left: {timeLeft}");

        ResetTimer();
        StopTimer();
    }

    // Start and stop timer
    private void StartTimer() => StartCoroutine(TimeCountdown(totalAmountOfSeconds));
    private void StopTimer()
    {
        StopAllCoroutines();
    }

    private void ResetTimer() => secondsLeft = totalAmountOfSeconds;

    private IEnumerator TimeCountdown(float length)
    {
        secondsLeft = length;

        float secondsInMinute = Mathf.FloorToInt(secondsLeft % 60);
        float minutesLeft = Mathf.FloorToInt(secondsLeft / 60);
        timerText.SetText(string.Format("{0:00}:{1:00}", minutesLeft, secondsInMinute));

        yield return new WaitForSeconds(2);

        while (secondsLeft > 0)
        {
            secondsLeft--;

            secondsInMinute = Mathf.FloorToInt(secondsLeft % 60);
            minutesLeft = Mathf.FloorToInt(secondsLeft / 60);
            timerText.SetText(string.Format("{0:00}:{1:00}", minutesLeft, secondsInMinute));
            AudioPlayer.PlaySound(tickSound, tickVolume);

            yield return second;
        }

        OnTimerUp?.Invoke();

        print("Timer Done!");
    }
}
