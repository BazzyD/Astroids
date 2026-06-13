using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource; // For punches, swooshes, UI
    [SerializeField] private AudioSource alarmSource; // For alarms and warnings
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;
    
    [Header("Music Playlists")]
    [SerializeField] private AudioClip[] menuTracks;
    [SerializeField] private AudioClip[] gameTracks;
    [SerializeField] private AudioClip[] gameOverTracks;
    [SerializeField] private AudioClip[] winTracks;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    private AudioSource activeSource;
    private AudioSource inactiveSource;
    private Coroutine fadeCoroutine;
    private AudioClip[] currentPlaylist;

    private void Awake(){
        // Standard Singleton Setup
        if (Instance == null) {
            activeSource = sourceA;
            inactiveSource = sourceB;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public void PlaySFX(AudioClip clip, float minPitch = 0.7f, float maxPitch = 1.1f){
        if (clip == null) return;

        // Randomize pitch
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        
        // Randomize volume slightly for extra organic feel
        float randomVolume = Random.Range(0.85f, 1.0f);

        // PlayOneShot allows multiple sounds to overlap (like hitting 3 enemies at once)
        sfxSource.PlayOneShot(clip, randomVolume);
    }
    public void PlayAlarm(AudioClip clip){
        if (clip == null) return;

        // PlayOneShot allows multiple sounds to overlap (like hitting 3 enemies at once)
        alarmSource.PlayOneShot(clip, 2.0f);
    }
    public void StopAlarm(){
        alarmSource.Stop();
    }

    void Update()
    {
        // Monitor if the active track is nearing its end to queue the next random track
        if (activeSource.isPlaying && activeSource.clip != null)
        {
            float timeRemaining = activeSource.clip.length - activeSource.time;
            if (timeRemaining <= fadeDuration && fadeCoroutine == null)
            {
                PlayNextRandomTrack();
            }
        }
    }

    public void PlayMenuBGMusic()
    {
        // Dont switch if we're already playing menu music
        if (currentPlaylist == menuTracks) return; 

        currentPlaylist = menuTracks;
        TransitionToNextTrack(GetRandomTrack(menuTracks));
    }
    public void PlayGameOverBGMusic()
    {
        // Dont switch if we're already playing game over music
        if (currentPlaylist == gameOverTracks) return; 

        currentPlaylist = gameOverTracks;
        TransitionToNextTrack(GetRandomTrack(gameOverTracks));
    }
    public void PlayWinBGMusic()
    {
        // Dont switch if we're already playing win music
        if (currentPlaylist == winTracks) return;

        currentPlaylist = winTracks;
        TransitionToNextTrack(GetRandomTrack(winTracks));
    }
    public void PlayGameBGMusic()
    {
        // If we are already playing game music AND something is actually spinning, do nothing
        if (currentPlaylist == gameTracks) return;

        currentPlaylist = gameTracks;
        TransitionToNextTrack(GetRandomTrack(gameTracks));
    }

    private void PlayNextRandomTrack()
    {
        if (currentPlaylist == null || currentPlaylist.Length == 0) return;
        TransitionToNextTrack(GetRandomTrack(currentPlaylist));
    }

    private AudioClip GetRandomTrack(AudioClip[] playlist)
    {
        if (playlist == null || playlist.Length == 0) return null;
        if (playlist.Length == 1) return playlist[0];

        // Pick a random track (ideally different from the currently playing one)
        int randomIndex = Random.Range(0, playlist.Length);
        return playlist[randomIndex];
    }

    private void TransitionToNextTrack(AudioClip newClip)
    {
        if (newClip == null) return;
    
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Crossfade(newClip, targetVolume: 0.7f));
    }

    private IEnumerator Crossfade(AudioClip newClip, float targetVolume)
    {
        // Setup the inactive source with the new track
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0;
        inactiveSource.Play();

        float timer = 0;
        float startActiveVolume = activeSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float progress = timer / fadeDuration;

            // Fade out the old, fade in the new
            activeSource.volume = Mathf.Lerp(startActiveVolume, 0, progress);
            inactiveSource.volume = Mathf.Lerp(0, targetVolume, progress);

            yield return null;
        }
        // Complete the transition
        activeSource.Stop();
        activeSource.volume = 0;
        // Swap the source identities
        (inactiveSource, activeSource) = (activeSource, inactiveSource);
        fadeCoroutine = null;
    }
}