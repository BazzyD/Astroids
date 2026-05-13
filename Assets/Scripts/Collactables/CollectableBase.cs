using UnityEngine;
using System.Collections;


public class CollectableBase : MonoBehaviour, ICollectable,IPoolable
{
    [SerializeField] private GameObject glow;
    [SerializeField] private GameObject visuals;
    [SerializeField] private string poolTag;
    [Header("Glow Expansion")]
    [SerializeField] private float expandSpeed = 100f;
    [SerializeField] private float maxRadius = 20f;
    private float currentRadius = 0f;
    private float initialRadius;

    private float disaperTimer = 0f;
    private float disaperDuration = 10f;
    private bool isFlashing = false;
    private void Start()
    {
        disaperTimer = disaperDuration;
        isFlashing = false;

        visuals.SetActive(true);
        glow.SetActive(true);
        currentRadius = glow.transform.localScale.x;
        initialRadius = glow.transform.localScale.x;
    }
    public void OnSpawn()
    {
        disaperTimer = disaperDuration;
        isFlashing = false;

        visuals.SetActive(true);
        glow.SetActive(true);
        currentRadius = glow.transform.localScale.x;
        initialRadius = glow.transform.localScale.x;
    }
    public void OnDespawn()
    {
        StopAllCoroutines();
        glow.SetActive(false);
        visuals.SetActive(false);
        glow.transform.localScale = new Vector3(initialRadius,initialRadius,1);
    }
    private void DespawnSelf()
    {
        if (ObjectPool.Instance == null) return;
        
        ObjectPool.Instance.Despawn(poolTag, gameObject);
    }
    private void Update()
    {
        if(disaperTimer <=0f) return;

        disaperTimer -= Time.deltaTime;

        if(disaperTimer <=3f && !isFlashing) {
            StartCoroutine(FlashRoutine());
        }

        if(disaperTimer <= 0f) {
            DespawnSelf();
        }
    }
    public virtual void Collect(GameObject collector)
    {
        StopAllCoroutines();
        glow.SetActive(true);
        GetComponent<Collider2D>().enabled = false; 
    
        // 2. Start the expansion animation
        StartCoroutine(GlowExpansionRoutine());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        Collect(other.gameObject);
    }
    private IEnumerator FlashRoutine() {
        isFlashing = true;

        while (disaperTimer > 0)
        {
            visuals.SetActive(!visuals.activeSelf);
            glow.SetActive(!glow.activeSelf);

            // Flicker faster as time runs out!
            float waitTime = (disaperTimer < 1f) ? 0.05f : 0.15f;
            yield return new WaitForSeconds(waitTime);
        }
        
        isFlashing = false;
    }
    private IEnumerator GlowExpansionRoutine() {
        if(visuals != null) visuals.SetActive(false);

        while (currentRadius < maxRadius)
        {
            currentRadius += expandSpeed * Time.deltaTime;
            glow.transform.localScale = new Vector3(currentRadius, currentRadius, 1f);
            
            yield return null; 
        }

        DespawnSelf();
        
    }




}