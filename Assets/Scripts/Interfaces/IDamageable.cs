using UnityEngine;

public interface IDamageable {
    void TakeDamage(Vector3 hitDirection, float damageAmount, int comboStep = 0); // comboStep is optional, default to 0 for non-combo attacks
}