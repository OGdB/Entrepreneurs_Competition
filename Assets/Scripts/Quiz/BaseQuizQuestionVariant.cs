using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseQuizQuestionVariant : MonoBehaviour
{
    #region Properties
    public QuizQuestionVariants ThisVariant { get => thisVariant; protected set => thisVariant = value; }
    private QuizQuestionVariants thisVariant;

    public QuizQuestionContainer CurrentQuestion { get => currentQuestion; private set => currentQuestion = value; }
    private QuizQuestionContainer currentQuestion;
    [SerializeField, Space(5)]
    private TextMeshProUGUI questionText;
    [SerializeField]
    private GameObject confirmButton;

    public Toggle[] AnswerToggles { get => answerToggles; private set => answerToggles = value; }
    [SerializeField] private Toggle[] answerToggles;
    private List<Toggle> unoccupiedToggles;  // The toggles that do not have an answer put into them yet.
    public List<Toggle> CorrectAnswerToggles { get => correctAnswerToggles; private set => correctAnswerToggles = value; }
    private List<Toggle> correctAnswerToggles = new();  // The toggles that do not have an answer put into them yet.

    public List<Answer> AnsweredList { get => answeredList; private set => answeredList = value; }
    List<Answer> answeredList = new();
    protected int amountOfCorrectAnswers = 0;
    protected int amountOfAnswersSelected = 0; // The amount of answers selected regardless of being correct.
    protected int amountOfCorrectAnswersSelected = 0;  // The amount of correct answers selected right now.
    
    // Have- or has the correct answer(s) been selected upon confirmation?
    public bool AllAnswersCorrect { get => correctAnswersSelected; private set => correctAnswersSelected = value; }

    private bool correctAnswersSelected = false;
    #endregion


    protected virtual void Awake() => unoccupiedToggles = new(AnswerToggles);

    protected virtual void Start()
    {
        if (ThisVariant == QuizQuestionVariants.Not_Set)
            Debug.LogError("The quiz question variant is not set! Set in Awake Override.");
    }

    public virtual void SetQuestion(QuizQuestionContainer question)
    {
        currentQuestion = question;

        questionText.SetText(question.question);
        amountOfCorrectAnswers = question.correctAnswers.Length;

        List<Answer> allAnswers = question.wrongAnswers.Concat(question.correctAnswers).ToList();
        // Set both the correct and wrong answers to a toggle randomly.
        foreach (Answer answer in allAnswers)
        {
            Toggle toggle = GetRandomAnswerToggle();
            toggle.GetComponentInChildren<TextMeshProUGUI>().SetText(answer.answer);
            toggle.onValueChanged.AddListener(delegate { AnswerToggled(toggle.isOn, answer); });

            if (answer.isCorrect)
                CorrectAnswerToggles.Add(toggle);
        }

        foreach (Toggle unusedToggle in unoccupiedToggles)
        {
            unusedToggle.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// An answer was clicked (not confirmed)
    /// </summary>
    /// <param name="isSelected">whether it was selected or de-selected</param>
    /// <param name="answer">the answer in the button</param>
    private void AnswerToggled(bool isSelected, Answer answer)
    {
        if (isSelected)
        {
            AnsweredList.Add(answer);
            amountOfAnswersSelected++;

            if (answer.isCorrect)
                amountOfCorrectAnswersSelected++;
        }
        else
        {
            AnsweredList.Remove(answer);
            amountOfAnswersSelected--;

            if (answer.isCorrect)
                amountOfCorrectAnswersSelected--;
        }

        // If all the correct answers were selected. (and no more than that)
        if (amountOfCorrectAnswersSelected == amountOfCorrectAnswers && // If all the correct answers are selected..
            amountOfAnswersSelected == amountOfCorrectAnswers)  // And the correct answers are the only ones selected.
        {
            AllAnswersCorrect = true;
        }

        confirmButton.SetActive(amountOfAnswersSelected > 0);
    }

    protected Toggle GetRandomAnswerToggle()
    {
        int randomInt = Random.Range(0, unoccupiedToggles.Count - 1);
        Toggle randomToggle = unoccupiedToggles[randomInt];
        unoccupiedToggles.RemoveAt(randomInt);

        return randomToggle;
    }

    /// <summary>
    /// Reset all the values of the question to be ready for a new question.
    /// </summary>
    public virtual void ResetToggles(bool resetToggles = true)
    {
        if (resetToggles)
        {
            unoccupiedToggles = new(AnswerToggles);
            CorrectAnswerToggles.Clear();
        }

        correctAnswersSelected = false;
        amountOfCorrectAnswersSelected = 0;
        amountOfAnswersSelected = 0;
        confirmButton.SetActive(false);
        AnsweredList.Clear();
        CurrentQuestion = null;

        foreach (var toggle in GetComponentsInChildren<OnUIInteractableInteraction>(true))
        {
            if (resetToggles)
            {
                toggle.gameObject.SetActive(true);
                toggle.GetComponentInChildren<TextMeshProUGUI>().SetText("");
            }

            toggle.ResetToggleSelectState();
        }
    }

    public string GetAnsweredAsPrettyString()
    {
        string prettyString = string.Empty;
        for (int a = 0; a < AnsweredList.Count; a++)
        {
            Answer answer = AnsweredList[a];
            prettyString += answer.answer;

            if (a+1 < answeredList.Count)
                prettyString += ", ";
        }


        return prettyString;
    }
}