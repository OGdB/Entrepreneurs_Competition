[System.Serializable]
public class Answer
{
    public string answer;
    public bool isCorrect;

    public Answer(string answer, bool isCorrect)
    {
        this.answer = answer;
        this.isCorrect = isCorrect;
    }
    public Answer(string answer, string isCorrect)
    {
        this.answer = answer;

        if (isCorrect == "TRUE")
        {
            this.isCorrect = true;
        }
        else if (isCorrect == "FALSE")
        {
            this.isCorrect = false;
        }
    }
    public Answer(string answer)
    {
        this.answer = answer;
    }
    public Answer() { }
}