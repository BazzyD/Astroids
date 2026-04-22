using UnityEngine;

public class HealCollectable : MonoBehaviour, ICollectable
{
    public void Collect(GameObject collector)
    {
        Health playerHealth = collector.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.Heal(20f);
            Destroy(gameObject);
        }
    }

    
        
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Use 'other' (the thing that entered the trigger) to check the tag
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject);
        }
    }

}
