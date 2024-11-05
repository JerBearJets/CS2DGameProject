using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    //Attack
    private const float attackDamage = 1f;
    [SerializeField] private float knockBackForce = 500f;

    [SerializeField]
    private AudioSource attackAudio;

    public GameObject player;
    public float speed;

    public float distance;

    public Animator animator;
    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //AI Follows Player, as well as rotates in the player's direction.

        distance = Vector2.Distance(transform.position, player.transform.position);

        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(Vector3.forward * (angle));

    }

    // Using collision, we will detect if a zombie collided with the player hitbox. If it does, we play the animation, take player damage, and continue.
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player")
        {
            attackAudio.Play();

            animator.SetTrigger("Attack");

            Collider2D collider = col.collider;
            IDamageable damageable = collider.GetComponent<IDamageable>();

            //Checks if Player is Damageable

            if (damageable != null)
            {
                //Stores the Direction the Enemy is Faceing
                Vector3 direction = (collider.transform.position - transform.position).normalized;

                //Adds Knockback force to the Direction
                Vector3 knockBack = direction * knockBackForce;

                //Passes Damage and Knockback to the Player
                damageable.Damage(attackDamage, knockBack);
            }

        }
    }

    
}
