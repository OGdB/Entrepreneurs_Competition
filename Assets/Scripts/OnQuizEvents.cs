using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OnQuizEvents : MonoBehaviour
{
    [SerializeField, Range(0f, 10f), Space(10)]
    private float onStartedDelay = 0f;
    [SerializeField, Space(10)]
    private UnityEvent onQuizStarted;
    private WaitForSeconds startWait;

    [SerializeField, Range(0f, 10f)]
    private float onFinishedDelay = 0f;
    [SerializeField]
    private UnityEvent onQuizFinished;
    private WaitForSeconds finishedWait;

    private void Awake()
    {
        startWait = new(onStartedDelay);
        finishedWait = new(onFinishedDelay);
    }
    private void OnEnable()
    {
        QuizManager.OnQuizStart += OnStarted;
        QuizManager.OnQuizOver += OnFinished;
    }
    private void OnDisable()
    {
        QuizManager.OnQuizStart -= OnStarted;
        QuizManager.OnQuizOver -= OnFinished;
    }

    private void OnStarted()
    {
        _ = StartCoroutine(OnStartedCR());

        IEnumerator OnStartedCR()
        {
            yield return startWait;
            onQuizStarted.Invoke();
        }
    }
    private void OnFinished()
    {
        _ = StartCoroutine(OnFinishedCR());

        IEnumerator OnFinishedCR()
        {
            yield return finishedWait;
            onQuizFinished.Invoke();
        }
    }
}
