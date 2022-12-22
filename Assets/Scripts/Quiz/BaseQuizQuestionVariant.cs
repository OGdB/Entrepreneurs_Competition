using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseQuizQuestionVariant : MonoBehaviour
{
    #region Properties
    public QuizQuestionVariants ThisVariant { get => thisVariant; protected set => thisVariant = value; }
    private QuizQuestionVariants thisVariant;

    [SerializeField, Space(5)]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private GameObject confirmButton;
    [SerializeField]
    protected Toggle[] answerToggles;
    private List<Toggle> unoccupiedToggles;

    protected int amountOfAnswersSelected = 0;
    protected int amountOfCorrectAnswers;
    protected int amountOfCorrectAnswersSelected = 0;
    protected int correctAnswerInt;
    public bool CorrectAnswersSelected { get => correctAnswersSelected; private set => correctAnswersSelected = value; }
    private bool correctAnswersSelected = false;
    #endregion


    protected virtual void Awake() => unoccupiedToggles = new(answerToggles);

    protected virtual void Start()
    {
        if (ThisVariant == QuizQuestionVariants.Not_Set)
            Debug.LogError("The quiz question variant is not set! Set in Awake Override.");
    }

    public virtual void SetQuestion(QuizQuestionContainer question)
    {
        questionText.SetText(question.question);
        amountOfCorrectAnswers = question.correctAnswers.Length;

        // Set both the correct and wrong answers to a toggle randomly.
        foreach (Answer answer in question.correctAnswers)
        {
            Toggle toggle = GetRandomAnswerToggle();

            toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(answer.answer);

            toggle.onValueChanged.AddListener(CorrectAnswerToggled);
            toggle.onValueChanged.AddListener(AnswerToggled);

        }
        foreach (Answer answer in question.wrongAnswers)
        {
            Toggle toggle = GetRandomAnswerToggle();

            toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(answer.answer);

            toggle.onValueChanged.AddListener(AnswerToggled);
        }

        foreach (Toggle unusedToggle in unoccupiedToggles)
        {
            unusedToggle.gameObject.SetActive(false);
        }
    }

    private void AnswerToggled(bool value)
    {
        amountOfAnswersSelected = value ? amountOfAnswersSelected += 1 : amountOfAnswersSelected -= 1;

        if (amountOfAnswersSelected > 0)
        {
            confirmButton.SetActive(true);
        }
        else
        {
            confirmButton.SetActive(false);
        }
    }

    private void CorrectAnswerToggled(bool value)
    {
        if (value)
        {
            amountOfCorrectAnswersSelected++;
        }
        else
        {
            amountOfCorrectAnswersSelected--;
        }

        CorrectAnswersSelected = amountOfCorrectAnswersSelected == amountOfCorrectAnswers;
    }

    protected Toggle GetRandomAnswerToggle()
    {
        int randomInt = Random.Range(0, unoccupiedToggles.Count - 1);
        Toggle randomToggle = unoccupiedToggles[randomInt];
        unoccupiedToggles.RemoveAt(randomInt);

        return randomToggle;
    }

    public virtual void ResetToggles()
    {
        unoccupiedToggles = new(answerToggles);
        correctAnswersSelected = false;
        amountOfCorrectAnswersSelected = 0;
        confirmButton.SetActive(false);

        foreach (var toggle in GetComponentsInChildren<OnUIInteractableInteraction>(true))
        {
            toggle.gameObject.SetActive(true);
            toggle.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            toggle.ResetToggle();
        }
    }
}