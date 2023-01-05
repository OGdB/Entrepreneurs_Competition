using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer _Instance;
    private static AudioSource source;
    private void Awake()
    {
        // Singleton
        {
            if (_Instance != null && _Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        source = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip, float volume = 1f) => source.PlayOneShot(clip, volume);
    public static void PlaySound(AudioClip clip, float delay, float volume = 1f)
    {
        source.clip = clip;
        source.volume = volume;
        source.PlayDelayed(delay);
    }
}
