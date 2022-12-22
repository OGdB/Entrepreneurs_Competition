public class QuizQuestion_StandardMC : BaseQuizQuestionVariant
{
    protected override void Awake()
    {
        base.Awake();
        ThisVariant = QuizQuestionVariants.Standard;
    }
}