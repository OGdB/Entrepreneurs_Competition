using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReadCSV
{
    #region Properties
    private const string googleSheetDocID = "1mEPQPC0KrTwqexnqOrtyq9ANklgoGTXdnbLw5m8LDQc";
    
    private const string url = "https://docs.google.com/spreadsheets/d/" + googleSheetDocID + "/export?format=csv";
    public static string DownloadedString { get => downloadedString; }
    private static string downloadedString = null;

    private const int questionVariantColumnIndex = 0; // The column in the spreadsheet the question-variant is located.
    private const int questionColumnIndex = 1; // The column in the spreadsheet where all the questions are located.
    private const int centerWordColumnIndex = 2; // The column in the spreadsheet where all the questions are located.
    private const int firstAnswerColumnIndex = 3; // The column in the spreadsheet where the answers begin.
    private const int questionCountColumnIndex = 14; // The column in the spreadsheet where the amount of questions in the database is stored.
    private const int answerCountColumnIndex = 16; // The column in the spreadsheet behind every question where the amount of answers for that question is stored.
    
    private const int firstQuestionRowIndex = 2; // The row in the spreadsheet in which the first question is located.
    #endregion

    public static IEnumerator DownloadGoogleSpreadSheet()
    {
        using UnityWebRequest webrequest = UnityWebRequest.Get(url);

        yield return webrequest.SendWebRequest();

        if (webrequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log($"Error: {webrequest.error}");
        }
        else
        {
            downloadedString = webrequest.downloadHandler.text;
        }
    }

    public static QuizQuestionContainer[] CSVTextToQuizQuestions(string CsvAsText, int amountOfQuestions)
    {
        List<string> lines = new(CsvAsText.Trim().Split('\n'));
        int amountOfQuestionsInDatabase = int.Parse(lines[0].Split(",")[questionCountColumnIndex]);  // the 14th cell of first row counts the amount of questions in the database.
        //Debug.Log($"Maximum int: {amountOfQuestionsInDatabase}");

        if (amountOfQuestions >= amountOfQuestionsInDatabase)
        {
            Debug.LogWarning("Asking for more questions than there are in the sheet!\n" +
                "Setting amount of questions to maximum available");
            amountOfQuestions = amountOfQuestionsInDatabase;
        }

        QuizQuestionContainer[] questions = new QuizQuestionContainer[amountOfQuestions];

        // To ensure I don't get the same question twice, I create a list of integers representing the question's integers,
        // When I pick out a question, I take it out as an option from the integer list.
        List<int> integerList = new();
        for (int j = firstQuestionRowIndex; j < amountOfQuestionsInDatabase; j++)
        {
            integerList.Add(j);
        }

        for (int i = 0; i < amountOfQuestions; i++)
        {
            // take a random number out of the integer list.
            int randomFromList = Random.Range(0, integerList.Count);
            int randomQuestionInt = integerList[randomFromList];
            integerList.RemoveAt(randomFromList);
            //Debug.Log($"Question column integer: {randomInt}");

            string randomLine = lines[randomQuestionInt]; // Trim to filter out empty fields. (not every question has same amount of answers)

            string[] fields = randomLine.Split(","/*, System.StringSplitOptions.RemoveEmptyEntries*/);

            List<Answer> correctAnswers = new();
            List<Answer> wrongAnswers = new();

            string questionVariant = fields[questionVariantColumnIndex];
            string question = fields[questionColumnIndex];
            string centerWord = fields[centerWordColumnIndex];

            string log = $"Question: {question}\n";
            int answerCount = int.Parse(fields[answerCountColumnIndex]);
            log += $"Answer count: {answerCount}\n";

            int testInt = 0;
            for (int a = firstAnswerColumnIndex; a < firstAnswerColumnIndex + answerCount * 2; a += 2)  // Starts at 3, == first 'Correct?' field, jumps 2 fields to jump to next correct field.
            {
                testInt++;
                string isCorrect = fields[a];
                string answer = fields[a + 1];

                if (answer != string.Empty) // If not blank answer
                {
                    Answer thisAnswer = new(answer, isCorrect);
                    log += $"Answer {a / 2}: {answer} ~ {isCorrect}\n";

                    if (thisAnswer.isCorrect)
                    {
                        correctAnswers.Add(thisAnswer);
                    }
                    else
                    {
                        wrongAnswers.Add(thisAnswer);
                    }
                }
            }

            //Debug.Log(log);

            int amountOfAnswers = correctAnswers.Count + wrongAnswers.Count;  // -8: all the 'Answer' headers that are not empty headers or the Correct field.

            questions[i] = new(questionVariant, question, centerWord, correctAnswers.ToArray(), amountOfAnswers, wrongAnswers.ToArray());
        }

        return questions;
    }
}
