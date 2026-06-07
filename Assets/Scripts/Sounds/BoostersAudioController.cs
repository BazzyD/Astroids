using System.Collections;
using UnityEngine;

public class BoostersAudioController : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioSource boostersSource;
    [SerializeField] private AudioClip boostersClip;

    [Header("Organic Randomization Settings")]
    [Range(0.5f, 1.5f)] [SerializeField] private float minPitch = 0.92f;
    [Range(0.5f, 1.5f)] [SerializeField] private float maxPitch = 1.08f;
    [Range(0.5f, 1f)]   [SerializeField] private float minVolume = 0.88f;
    [Range(0.5f, 1f)]   [SerializeField] private float maxVolume = 1.00f;
    private Coroutine boostersRoutine;
    private bool isMooving = false;

    [SerializeField]private float loopStart = 3.0f;
    [SerializeField]private float loopEnd = 18.0f;


    public void StartBoosters()
    {
        if (isMooving) return;
        isMooving = true;

        if (boostersRoutine != null) StopCoroutine(boostersRoutine);
        boostersRoutine = StartCoroutine(BoostersPlaybackRoutine());
    }

    public void StopBoosters()
    {
        if (!isMooving) return;
        isMooving = false;
        boostersSource.Stop();
    }

    private IEnumerator BoostersPlaybackRoutine()
    {
        boostersSource.clip = boostersClip;
        boostersSource.loop = false;

        boostersSource.pitch = Random.Range(minPitch, maxPitch);
        boostersSource.volume = Random.Range(minVolume, maxVolume);
        
        boostersSource.Play();

        while (isMooving)
        {
            if (boostersSource.time >= loopEnd)
            {
                boostersSource.time = loopStart;
            }
            yield return null;
        }
    }
}