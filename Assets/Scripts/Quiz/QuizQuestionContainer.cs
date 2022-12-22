using UnityEngine;

public class QuizQuestionContainer
{
    public string questionVariant;
    public string question;
    public string centerWord;
    [Space(5)]
    public Answer[] correctAnswers;
    [Space(5)]
    public Answer[] wrongAnswers;
    [Space(5)]
    public int answerCount;

    public QuizQuestionContainer(string questionVariant, string question, string centerWord, Answer[] correctAnswers, int answerCount, Answer[] wrongAnswers)
    {
        this.questionVariant = questionVariant;
        this.question = question;
        this.centerWord = centerWord;
        this.correctAnswers = correctAnswers;
        this.wrongAnswers = wrongAnswers;
        this.answerCount = answerCount;
    }
}

