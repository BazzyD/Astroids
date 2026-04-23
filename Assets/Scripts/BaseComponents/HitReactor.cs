using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]

public class HitReactor : MonoBehaviour{
    private static readonly WaitForSeconds _waitForSeconds0_1 = new(0.1f);
    [Header("Visuals")]
    [SerializeField] private Material defaultMaterial; 
    [SerializeField] private Material flashMaterial;

    [Header("Audio")]
    [SerializeField] private AudioClip[] hitSound;


    private Coroutine activeFlashRoutine;
    private SpriteRenderer spriteRenderer;
    private Health health;

    private void Awake(){
        health = GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer == null) 
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
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

        int randomIdx = Random.Range(0, hitSound.Length);
        if(hitSound.Length > 0)
            AudioManager.Instance.PlaySFX(hitSound[randomIdx]);
        activeFlashRoutine =StartCoroutine(HitFlashRoutine());
    }
    private IEnumerator HitFlashRoutine() {
        spriteRenderer.material = flashMaterial;
        yield return _waitForSeconds0_1;
        spriteRenderer.material = defaultMaterial;
    }
}