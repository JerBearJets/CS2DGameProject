using UnityEngine;

public class MouseFollower : MonoBehaviour
{

    private Vector3 velocity = Vector3.zero;
    //Allows changing the damping of the MouseFollower follow
    [SerializeField] private float damping;
    //Character Controller script reference
    private CharacterController2D characterController2D;
    


    private void Awake()
    {
        characterController2D = GameObject.Find("Player").GetComponent<CharacterController2D>();
    }

    private void FixedUpdate()
    {
        if (characterController2D.isAttacking == true)
        {
            //Prevents Follower if the Player is Attacking
        }
        else
        {
            //Sets the MouseFollower to follow the Mouse
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Prevents MouseFollower from moving on Z Axis
            targetPosition.z = transform.position.z;
            //Moves the MouseFollower
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
        }
    }
}
