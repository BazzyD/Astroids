using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]

public class HitReactor : MonoBehaviour{
    
    [Header("Visuals")]
    [SerializeField] private Material defaultMaterial; 
    [SerializeField] private Material flashMaterial;

    [Header("Audio")]
    [SerializeField] private AudioClip[] lightHitSounds; // Array for multiple sounds!
    [SerializeField] private AudioClip[] heavyHitSound;
    [SerializeField] private AudioClip[] landHitSound;

    private Coroutine activeFlashRoutine;
    private SpriteRenderer spriteRenderer;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnEnable()
    {
        health.OnTakeDamage += TakeDamage;
    }
    private void OnDisable()
    {
        health.OnTakeDamage -= TakeDamage;
    }

    public void TakeDamage(Vector3 hitDirection, float damageAmount, int comboStep = 0){

        if (activeFlashRoutine != null) {
            StopCoroutine(activeFlashRoutine);
        }

        if(comboStep == 3){
            if (heavyHitSound.Length > 0) {
                int randomIdx = Random.Range(0, heavyHitSound.Length);
                AudioManager.Instance.PlaySFX(heavyHitSound[randomIdx]);
            }
            activeFlashRoutine = StartCoroutine(KnockUpCoroutine()); // Knock up on finisher

        }
        else{
            // Standard small flinch pushback
            if (lightHitSounds.Length > 0) {
                int randomIdx = Random.Range(0, lightHitSounds.Length);
                AudioManager.Instance.PlaySFX(lightHitSounds[randomIdx]);
            } 
            activeFlashRoutine =StartCoroutine(HitFlashRoutine());
        }
    }

    private IEnumerator HitFlashRoutine() {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = defaultMaterial;
    }

    private IEnumerator KnockUpCoroutine(){
        spriteRenderer.material = flashMaterial;

        float elapsedTime = 0f;
        while (elapsedTime < health.TotalFlightTime)
        {
            elapsedTime += Time.unscaledDeltaTime;; 
            float t = elapsedTime / health.TotalFlightTime;
            
            // Every 0.05 seconds, swap between Solid White and Normal
            spriteRenderer.material = (Time.time % 0.1f > 0.05f) ? flashMaterial : defaultMaterial;

            yield return null;
        }

        // Finalize
        spriteRenderer.material = defaultMaterial;
        if (landHitSound.Length > 0) {
            int randomIdx = Random.Range(0, landHitSound.Length);
            AudioManager.Instance.PlaySFX(landHitSound[randomIdx]);
        }
    }
}
