using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour, IPoolable
{
    private ParticleSystem ps;
    [SerializeField] protected string poolTag;
    protected void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(CheckIfAlive());
    }
    public virtual void OnDespawn()
    {
    }

    public virtual void OnSpawn()
    {
    }
    protected IEnumerator CheckIfAlive()
    {
        // Wait while the particle system is still active
        // Setting 'true' in IsAlive checks all child particle systems too
        while (ps != null && ps.IsAlive(true))
        {
            yield return new WaitForSeconds(0.5f); // Check every half second to save performance
        }

        if(ObjectPool.Instance == null) gameObject.SetActive(false);
        else
            ObjectPool.Instance.Despawn(poolTag, this.gameObject);
    }
}
