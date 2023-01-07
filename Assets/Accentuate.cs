using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accentuate : MonoBehaviour
{
    public static Accentuate Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void AccentuateObject(GameObject obj, float targetAlpha)
    {
        obj.layer = LayerMask.NameToLayer("Accentuated");
        _ = Singleton.StartCoroutine(SceneTransition.Fade(targetAlpha));
    }
    public static void UnAccentuateObject(GameObject obj)
    {
        // Wait until the fadeout is done before putting the object back in Default.
        _ = Singleton.StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            yield return Singleton.StartCoroutine(SceneTransition.Fade(0f));

            obj.layer = LayerMask.NameToLayer("Default");
        }
    }

    public static void AccentuateObject(List<GameObject> list, float targetAlpha)
    {
        foreach (var obj in list)
        {
            obj.layer = LayerMask.NameToLayer("Accentuated");
        }
    }
    public static void UnaccentuateObject(List<GameObject> list)
    {
        // Wait until the fadeout is done before putting the object back in Default.
        _ = Singleton.StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            yield return Singleton.StartCoroutine(SceneTransition.Fade(0f));

            foreach (var obj in list)
            {
                obj.layer = LayerMask.NameToLayer("Default");
            }
        }
    }


    public static void StartAccentuation(float targetAlpha)
    {
        _ = Singleton.StartCoroutine(SceneTransition.Fade(targetAlpha));
    }
    public static void StopAccentuation()
    {
        _ = Singleton.StartCoroutine(SceneTransition.Fade(0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            FindObjectOfType<Group>().GroupBuilding.AccentuateBuilding();
            StartAccentuation(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopAccentuation();
            FindObjectOfType<Group>().GroupBuilding.UnaccentuateBuilding();
        }
    }
}
