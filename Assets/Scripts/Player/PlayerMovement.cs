using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    private Animator _animator; 

    public Transform cameraTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>(); 
    }

    public void Move(Vector2 input)
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 direction = forward * input.y + right * input.x;
        direction = Vector3.ClampMagnitude(direction, 1f);

        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);

        Vector3 localDir = transform.InverseTransformDirection(direction);
        _animator.SetFloat("VelocityX", localDir.x, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityZ", localDir.z, 0.1f, Time.deltaTime);
    }
}