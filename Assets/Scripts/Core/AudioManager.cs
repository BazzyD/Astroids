using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton so anyone can access it: AudioManager.Instance.PlaySFX(...)
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource; // For punches, swooshes, UI
    [SerializeField] private AudioSource musicSource; // For background music

    private void Awake()
    {
        // Standard Singleton Setup
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the manager alive between levels!
        } else {
            Destroy(gameObject);
        }
    }

    // The Wrapper Function
    public void PlaySFX(AudioClip clip, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (clip == null) return;

        // Randomize pitch
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        
        // Randomize volume slightly for extra organic feel
        float randomVolume = Random.Range(0.85f, 1.0f);

        // PlayOneShot allows multiple sounds to overlap (like hitting 3 enemies at once)
        sfxSource.PlayOneShot(clip, randomVolume);
    }
}