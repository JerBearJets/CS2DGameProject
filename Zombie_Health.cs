using UnityEngine;

public class Zombie_Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    public float currentHealth;

    

    void Start()
    {
        currentHealth = maxHealth;
    }

    //Applies Damage to Enemy
    public void Damage(float attackDamage, Vector3 knockBack)
    {
        currentHealth -= attackDamage;


        //Ensures Health does not fall below 0
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
