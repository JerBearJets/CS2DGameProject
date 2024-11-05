using UnityEngine;

//Interface for dealing Damage to Damagable Enemies
public interface IDamageable
{
    public void Damage(float attackDamage, Vector3 knockBack);
}
