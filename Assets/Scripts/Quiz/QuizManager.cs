using System.Collections;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    #region Properties
    [Space(10)]
    public QuizQuestionVariants currentQuestionVariant;
    [SerializeField, Space(10)]
    private int amountOfQuestionsToAsk = 3;
    [SerializeField]
    private int scorePerAnswer = 10;
    private int votedToStart = 0;

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

    [SerializeField, Header("Assignables")]
    private TextMeshProUGUI questionNumberText;
    [SerializeField]
    private TextMeshProUGUI answersCorrectText;
    [SerializeField]
    private TextMeshProUGUI answersWrongText;
    [SerializeField]
    private GameObject readyButton;

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

    public int AmountOfConfirmedVotes { get => amountOfConfirmedVotes; set => amountOfConfirmedVotes = value; }
    private int amountOfConfirmedVotes = 0;
    
    public int VotedCorrectly { get => votedCorrectly; set => votedCorrectly = value; }
    private int votedCorrectly = 0;
    #endregion

    private void OnEnable() => QuizTimer.OnTimerUp += OnQuizFinished;
    private void OnDisable() => QuizTimer.OnTimerUp -= OnQuizFinished;

    public void VotedToStartQuiz()
    {
        votedToStart++;
        DBManager.Singleton.NextUser();
        if (votedToStart >= DBManager.AmountOfUsers)
        {
            readyButton.SetActive(false);
            _ = StartCoroutine(StartQuizCR());
        }

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

    /// <summary>
    /// Confirm button pressed, meaning 1 user has made their vote.
    /// </summary>
    public void OnAnswerConfirmation()
    {
        AmountOfConfirmedVotes++;

        print($"{DBManager.Singleton.currentUser.Name} voted for {quizQuestionVariant.GetAnsweredAsPrettyString()}");

        // Was the vote correct?
        if (quizQuestionVariant.AllAnswersCorrect)
        {
            VotedCorrectly++;
        }

        // If each member has voted, check if the majority voted correctly.
        if (AmountOfConfirmedVotes == DBManager.AmountOfUsers)
        {
            // If more than half of the group voted for the correct answer, they get points.
            float percentageVotedCorrect = votedCorrectly / (float)AmountOfConfirmedVotes;
            print($"Percentage voted correctly = {percentageVotedCorrect * 100 }%");
            if (percentageVotedCorrect >= 0.5f)
            {
                answeredCorrect++;
                AudioPlayer.PlaySound(clip: correctSound);
            }
            else
            {
                answeredWrong++;
                AudioPlayer.PlaySound(clip: wrongSound);
            }

            // After everyone voted, reset these values for the next question.
            amountOfConfirmedVotes= 0;
            VotedCorrectly = 0;
            
            // Then, move to the next question.
            if (currentQuestionInt < questions.Length - 1)
            {
                ClearQuestionFields();
                currentQuestionInt++;

                questionNumberText.SetText($"Question {currentQuestionInt + 1} / {questions.Length}");

                quizQuestionVariant.ResetToggles();
                SetToCurrentQuestion();
                return;
            }
            else // Quiz finished
            {
                OnQuizFinished();
            }
        }

        // Reset the toggles for the next user to vote
        quizQuestionVariant.ResetToggles(resetToggles: false);
        DBManager.Singleton.NextUser();
    }

    private void OnQuizFinished()
    {
        _ = StartCoroutine(QuizFinishedCR());

        IEnumerator QuizFinishedCR()
        {
            OnQuizOver?.Invoke();

            int oldScore = DBManager.Singleton.Score;
            // Calculate score and increase in DBManager.
            int scoreIncrease = CalculateScore(scorePerAnswer: scorePerAnswer, amountOfCorrectAnswers: answeredCorrect);
            DBManager.Singleton.IncreaseScore(scoreIncrease);  

            QuizTimer.OnTimerUp -= OnQuizFinished;

            answersCorrectText.SetText($"Answers Correct: {answeredCorrect}");
            answersWrongText.SetText($"Answers Wrong: {answeredWrong}");
            SceneTransition.TransitionToScene("City", LoadSceneMode.Additive);

            yield return new WaitForSeconds(1.5f);

            AudioPlayer.PlaySound(clip: quizOverSound);
        }
    }

    /// <summary>
    /// Get a score dependent on amount of answers correct.
    /// </summary>
    private int CalculateScore(int scorePerAnswer, int amountOfCorrectAnswers)
    {
        int scoreIncrease = scorePerAnswer * amountOfCorrectAnswers;
        return scoreIncrease;
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
