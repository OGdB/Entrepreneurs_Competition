using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    #region Properties
    [Space(10)]
    public QuizQuestionVariants currentQuestionVariant;
    [SerializeField, Space(10)]
    private int amountOfQuestionsToAsk = 3;

    [Header("Question Variants"), SerializeField]
    private GameObject standard;
    [SerializeField]
    private GameObject webConnections;

    private GameObject currentActivatedQuestionVariantObject;

    [SerializeField, Header("Sounds")]
    private AudioClip wrongSound;
    [SerializeField]
    private AudioClip correctSound;    
    [SerializeField]
    private AudioClip quizOverSound;

    [SerializeField, Header("Text fields")]
    private TextMeshProUGUI questionNumberText;
    [SerializeField, Space(5)]
    private TextMeshProUGUI answersCorrectText;
    [SerializeField]
    private TextMeshProUGUI answersWrongText;

    private int answeredCorrect = 0;
    private int answeredWrong = 0;

    private QuizQuestionContainer[] questions;
    private BaseQuizQuestionVariant quizQuestionVariant;
    private int currentQuestionInt = 0;

    // Events
    public delegate void QuizStart();
    public static QuizStart OnQuizStart;
    
    public delegate void QuizOver();
    public static QuizOver OnQuizOver;
    #endregion

    private void OnEnable() => QuizTimer.OnTimerUp += QuizFinished;
    private void OnDisable() => QuizTimer.OnTimerUp -= QuizFinished;

    public void StartQuiz()
    {
        _ = StartCoroutine(StartQuizCR());

        IEnumerator StartQuizCR()
        {
            yield return ReadCSV.DownloadGoogleSpreadSheet();

            questions = ReadCSV.CSVTextToQuizQuestions(ReadCSV.DownloadedString, amountOfQuestionsToAsk);

            questionNumberText.SetText($"Question 1 / {questions.Length}");

            System.Random rdm = new();
            rdm.Shuffle(questions);

            OnQuizStart?.Invoke();

            SetToCurrentQuestion();
        }
    }

   

    private GameObject GetQuestionVariantObject(QuizQuestionVariants questionVariant)
    {
        switch (questionVariant)
        {
            case QuizQuestionVariants.Not_Set:
                Debug.LogError("ERROR! The question variant is not set!");
                return null;
                
            case QuizQuestionVariants.Standard:
                return standard;

            case QuizQuestionVariants.Web:
                return webConnections;

            default: return null;
        }
    }

    private void SetToCurrentQuestion()
    {
        QuizQuestionContainer currentQuestion = questions[currentQuestionInt];

        if (System.Enum.TryParse(currentQuestion.questionVariant, out QuizQuestionVariants yourEnum))
        {
            currentQuestionVariant = yourEnum;
        }
        else
        {
            Debug.LogError("Error in parsing question variant string to enumerator!");
        }

        // Activate the gameobject for the question variant accordingly.

        if (currentActivatedQuestionVariantObject != null && currentActivatedQuestionVariantObject.activeInHierarchy)
            currentActivatedQuestionVariantObject.SetActive(false);

        currentActivatedQuestionVariantObject = GetQuestionVariantObject(currentQuestionVariant);
        currentActivatedQuestionVariantObject.SetActive(true);
        quizQuestionVariant = currentActivatedQuestionVariantObject.GetComponent<BaseQuizQuestionVariant>();
        quizQuestionVariant.SetQuestion(currentQuestion);
    }

    public void OnAnswerConfirmation()
    {
        if (quizQuestionVariant.CorrectAnswersSelected)
        {
            answeredCorrect++;
            AudioPlayer.PlaySound(clip: correctSound);
        }
        else
        {
            answeredWrong++;
            AudioPlayer.PlaySound(clip: wrongSound);
        }


        ClearQuestionFields();

        if (currentQuestionInt < questions.Length - 1)
        {
            currentQuestionInt++;

            questionNumberText.SetText($"Question {currentQuestionInt + 1} / {questions.Length}");

            SetToCurrentQuestion();
        }
        else // Quiz finished
        {
            QuizFinished();
        }
    }

    private void QuizFinished()
    {
        _ = StartCoroutine(QuizFinishedCR());

        IEnumerator QuizFinishedCR()
        {
            OnQuizOver?.Invoke();
            QuizTimer.OnTimerUp -= QuizFinished;

            answersCorrectText.SetText($"Answers Correct: {answeredCorrect}");
            answersWrongText.SetText($"Answers Wrong: {answeredWrong}");

            yield return new WaitForSeconds(1.5f);

            AudioPlayer.PlaySound(clip: quizOverSound);
        }
    }

    private void ClearQuestionFields()
    {
        quizQuestionVariant.ResetToggles();
        standard.SetActive(false);
        webConnections.SetActive(false);
    }

    public void Reset()
    {
        answeredCorrect = 0;
        answeredWrong = 0;
        currentQuestionInt = 0;
    }
}
