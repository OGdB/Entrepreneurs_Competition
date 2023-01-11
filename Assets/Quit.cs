using UnityEngine;

public class Quit : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.enabled = !canvas.enabled;
        }
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif    
    }
}
