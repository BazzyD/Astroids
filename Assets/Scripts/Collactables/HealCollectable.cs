using UnityEngine;

public class HealCollectable : CollectableBase
{
    public override void Collect(GameObject collector)
    {
        if(!collector.TryGetComponent(out Health playerHealth)) return;

        playerHealth.Heal(20f);

        base.Collect(collector);
    }
}
