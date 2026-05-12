using UnityEngine;
using System.Collections;


public class CollectableBase : MonoBehaviour, ICollectable
{
    private SpriteRenderer spriteRenderer;
    private float disaperTimer = 0f;
    private float disaperDuration = 10f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        disaperTimer = disaperDuration;
    }
    private void Update()
    {
        if(disaperTimer <=0f) return;

        disaperTimer -= Time.deltaTime;
        if(disaperTimer <=3f) {
            StartCoroutine(DisaperRoutine());
        }
        if(disaperTimer <= 0f) Destroy(gameObject);
    }
    public virtual void Collect(GameObject collector)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Collect(other.gameObject);
    }
    private IEnumerator DisaperRoutine() {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;
    }


}