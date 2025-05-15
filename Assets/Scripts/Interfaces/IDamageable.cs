using UnityEngine;

public interface IDamageable
{
    public int OnHit(int damage, Vector2 Knockback);
}
