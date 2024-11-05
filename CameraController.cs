using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    //Allows the selection of a Target for the Camera to follow
    public Transform target;

    private Vector3 velocity = Vector3.zero;

    //Allows changing the damping of the Camera follow

    [SerializeField] private float damping;

    private void FixedUpdate()
    {
        //Sets the Camera to follow the set Target

        Vector3 targetPosition = target.position;

        //Prevents Camera from moving on Z Axis

        targetPosition.z = transform.position.z;

        //Check Camera Bounds
        if (targetPosition.x > 91)
        {
            targetPosition.x = 91;
        }
        if (targetPosition.y > 78)
        {
            targetPosition.y = 78;
        }
        if (targetPosition.x < -93)
        {
            targetPosition.x = -93;
        }
        if (targetPosition.y < -89)
        {
            targetPosition.y = -89;
        }

        //Moves the Camera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
    }
}
