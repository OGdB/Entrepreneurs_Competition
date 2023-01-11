using UnityEngine;
using UnityEngine.UI;

public class Mute : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource musicSource2;
    public Image muteImage;

    public Color mutedColor;

    public void SwitchMute()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
            musicSource2.Pause();
            //print("Mute!");
            muteImage.color = mutedColor;
        }
        else
        {
            musicSource.UnPause();
            musicSource2.UnPause();
            //print("UnMute!");

            muteImage.color = Color.white;
        }
    }
}
