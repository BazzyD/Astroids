using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]

public class HitReactor : MonoBehaviour{
    private static readonly WaitForSeconds _waitForSeconds0_1 = new(0.1f);
    [Header("Visuals")]
    [SerializeField] private Material defaultMaterial; 
    [SerializeField] private Material flashMaterial;

    [Header("Audio")]
    [SerializeField] private AudioClip[] heavyHitSound;
    [SerializeField] private AudioClip[] landHitSound;

    private Coroutine activeFlashRoutine;
    private SpriteRenderer spriteRenderer;
    private Health health;

    private void Awake(){
        health = GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnEnable(){
        health.OnTakeDamage += TakeDamage;
    }
    private void OnDisable(){
        health.OnTakeDamage -= TakeDamage;
    }

    public void TakeDamage(float damageAmount){
        if (activeFlashRoutine != null) {
            StopCoroutine(activeFlashRoutine);
        }

        int randomIdx = Random.Range(0, heavyHitSound.Length);
        AudioManager.Instance.PlaySFX(heavyHitSound[randomIdx]);
        activeFlashRoutine =StartCoroutine(HitFlashRoutine());
    }
    private IEnumerator HitFlashRoutine() {
        spriteRenderer.material = flashMaterial;
        yield return _waitForSeconds0_1;
        spriteRenderer.material = defaultMaterial;
    }
}