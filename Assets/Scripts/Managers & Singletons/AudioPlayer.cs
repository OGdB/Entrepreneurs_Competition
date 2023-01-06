using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Singleton;
    private AudioSource source;
    private void Awake()
    {
        // Singleton
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        source = GetComponent<AudioSource>();
    }
    private void OnDestroy()
    {
        Singleton = null;
    }

    public static void PlaySound(AudioClip clip, float volume = 1f) => Singleton.source.PlayOneShot(clip, volume);
    public static void PlaySound(AudioClip clip, float delay, float volume = 1f)
    {
        Singleton.source.clip = clip;
        Singleton.source.volume = volume;
        Singleton.source.PlayDelayed(delay);
    }
}
