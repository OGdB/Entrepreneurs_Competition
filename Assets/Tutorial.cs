using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool hasGoneThroughTutorial = false;
    [SerializeField]
    private PopupMessageCity popUpHandler;
    [SerializeField]
    private float amountOfReadingTime = 4f;
    [SerializeField]
    private float timeBetweenExplanations = 2f;

    [TextArea(1,4)]
    public string[] explanations;
    public int currentExplanationInt = 0;

    WaitForSeconds readingTime;
    WaitForSeconds timeBetween;

    private void Awake()
    {
        readingTime = new(amountOfReadingTime);
        timeBetween = new(timeBetweenExplanations);
    }
    private void Start()
    {
        if (!hasGoneThroughTutorial)
        {
            _ = StartCoroutine(MainTutorial());
        }
        else
        {
            string lastMessage = "This is all we have for the demo. Thank you for trying it out!";
            _ = StartCoroutine(popUpHandler.PopUpCR(lastMessage, amountOfReadingTime, 2.5f));
        }
    }

    public IEnumerator SetNewTutorialText(string newTutorialText)
    {
        yield return StartCoroutine(popUpHandler.PopUpCR(newTutorialText, amountOfReadingTime));

        yield return timeBetween;
    }

    private IEnumerator MainTutorial()
    {
        yield return new WaitForSeconds(5f);
        // Panning
        yield return StartCoroutine(SetNewTutorialText(explanations[currentExplanationInt]));

        currentExplanationInt++;

        // Show buildings
        yield return new WaitForSeconds(3f);
        CompanyBuilding[] companyBuildings = FindObjectsOfType<CompanyBuilding>();
        foreach (var companyBuilding in companyBuildings)
        {
            companyBuilding.AccentuateBuilding();
        }
        yield return null;
        yield return StartCoroutine(SetNewTutorialText(explanations[currentExplanationInt]));

        foreach (var companyBuilding in companyBuildings)
        {
            companyBuilding.UnaccentuateBuilding();
        }

        yield return new WaitForSeconds(4f);

        hasGoneThroughTutorial = true;
        GameManager.Singleton.TriggerQuiz();
    }

    
}
