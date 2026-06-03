using System.Collections;
using UnityEngine;

public class LaserAudioController : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource laserSource;
    [SerializeField] private AudioClip laserClip;

    [Header("Organic Randomization Settings")]
    [Range(0.5f, 1.5f)] [SerializeField] private float minPitch = 0.92f;
    [Range(0.5f, 1.5f)] [SerializeField] private float maxPitch = 1.08f;
    [Range(0.5f, 1f)]   [SerializeField] private float minVolume = 0.88f;
    [Range(0.5f, 1f)]   [SerializeField] private float maxVolume = 1.00f;
    private Coroutine laserRoutine;
    private bool isFiring = false;

    // Timestamps matching your exact audio markers
    [SerializeField]private float loopStart = 1.5f;
    [SerializeField]private float loopEnd = 3.5f;
    [SerializeField]private float endingStart = 4.0f;

    /// <summary>
    /// Call this from your Weapon script when the player presses the fire button.
    /// </summary>
    public void StartLaser()
    {
        if (isFiring) return; // Prevent restarting if already holding down fire
        isFiring = true;

        if (laserRoutine != null) StopCoroutine(laserRoutine);
        laserRoutine = StartCoroutine(LaserPlaybackRoutine());
    }

    /// <summary>
    /// Call this from your Weapon script when the player releases the fire button.
    /// </summary>
    public void StopLaser()
    {
        if (!isFiring) return;
        isFiring = false;
        // The coroutine will notice this change and handle the outro transition automatically
    }

    private IEnumerator LaserPlaybackRoutine()
    {
        laserSource.clip = laserClip;
        laserSource.loop = false; // We handle looping manually via code

        laserSource.pitch = Random.Range(minPitch, maxPitch);
        laserSource.volume = Random.Range(minVolume, maxVolume);
        
        laserSource.Play();
        laserSource.time = loopStart;

        // --- Intro & Loop Phase ---
        while (isFiring)
        {
            // If the audio timeline passes 4.0 seconds, snap it back to 0.5 seconds
            if (laserSource.time >= loopEnd)
            {
                laserSource.time = loopStart;
            }
            yield return null; // Check every frame
        }

        // --- Outro Phase ---
        // As soon as isFiring is set to false, drop out of the loop and skip straight to the outro
        laserSource.time = endingStart; 
    }
}