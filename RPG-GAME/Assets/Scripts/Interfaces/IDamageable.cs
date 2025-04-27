using UnityEngine;

public interface IDamageable
{
    public void OnHit(int damage, Vector2 Knockback);
}
