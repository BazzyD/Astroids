using UnityEngine;

public class HealCollectable : CollectableBase
{
    [SerializeField] private float healAmount = 5f;
    public override void Collect(GameObject collector)
    {
        if(!collector.TryGetComponent(out Health playerHealth)) return;

        playerHealth.Heal(healAmount);

        base.Collect(collector);
    }
}
