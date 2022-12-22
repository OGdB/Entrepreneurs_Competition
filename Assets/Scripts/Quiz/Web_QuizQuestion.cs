using UnityEngine;

public class Web_QuizQuestion : BaseQuizQuestionVariant
{
    [SerializeField]
    private GameObject centerWordGO;
    
    protected override void Awake()
    {
        base.Awake();
        ThisVariant = QuizQuestionVariants.Web;
    }

    public override void SetQuestion(QuizQuestionContainer question)
    {
        base.SetQuestion(question);
        centerWordGO.GetComponent<TMPro.TextMeshProUGUI>().SetText(question.centerWord);
    }
}
