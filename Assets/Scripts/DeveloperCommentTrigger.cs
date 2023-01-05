using UnityEngine;
using UnityEngine.EventSystems;

public class DeveloperCommentTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(2, 4)]
    public string comment;

    // For UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        DeveloperCommentSingleton.SetDeveloperComment(comment);
        DeveloperCommentSingleton.OnTriggerEntered();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeveloperCommentSingleton.OnTriggerExited();
        DeveloperCommentSingleton.SetDeveloperComment("");
    }

    // These are for 3D objects with Colliders. The Collider needs to be on the same object and of course the proper size.
    private void OnMouseEnter()
    {
        DeveloperCommentSingleton.SetDeveloperComment(comment);
        DeveloperCommentSingleton.OnTriggerEntered();
    }
    private void OnMouseExit()
    {
        DeveloperCommentSingleton.OnTriggerExited();
        DeveloperCommentSingleton.SetDeveloperComment("");
    }
}
