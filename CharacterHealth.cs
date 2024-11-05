using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider healthSlider1;

    [SerializeField] private Slider healthSlider2;

    [SerializeField] private Slider healthSlider3;

    [SerializeField] private Slider healthSlider4;

    private const float maxHealth = 4f;

    public float currentHealth;

    private bool immuneState = false;

    private float immuneTimer = 0f;

    private const float immuneDuration = 3f;

    private Rigidbody2D playerRigidbody;

    private CharacterController2D characterController2D;

    public Vector3 isKnockedBack;

    [SerializeField] private AudioSource deathAudio;

    void Start()
    {
        currentHealth = maxHealth;

        healthSlider1 = GameObject.Find("Health1").GetComponent<Slider>();

        healthSlider2 = GameObject.Find("Health2").GetComponent<Slider>();

        healthSlider3 = GameObject.Find("Health3").GetComponent<Slider>();

        healthSlider4 = GameObject.Find("Health4").GetComponent<Slider>();

        characterController2D = GameObject.Find("Player").GetComponent<CharacterController2D>();

        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    //Applies Damage to Player
    public void Damage(float attackDamage, Vector3 knockBack)
    {
        if (!immuneState)
        {
            immuneState = true;

            currentHealth -= attackDamage;

            isKnockedBack = knockBack;

            //Ensures Health doesn't go below 0 or above Max

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 3)
            {
                healthSlider4.value = 0;
            }
            if (currentHealth <= 2)
            {
                healthSlider3.value = 0;
            }
            if (currentHealth <= 1)
            {
                healthSlider2.value = 0;
            }
            if (currentHealth <= 0)
            {
                healthSlider1.value = 0;
                Die();
            }   
        }
    }

    private void Update()
    {
        if (immuneState)
        {
            immuneTimer += Time.deltaTime;

            if (immuneTimer >= immuneDuration)
            {
                immuneState = false;

                immuneTimer = 0f;
            }
        }
        else
        {
            immuneTimer = 0f;
        }
    }

    //What occurs when Player Dies
    private void Die()
    {
        SceneManager.LoadScene("Game Over", LoadSceneMode.Additive);

        deathAudio.Play();

        gameObject.SetActive(false);
    }
}

