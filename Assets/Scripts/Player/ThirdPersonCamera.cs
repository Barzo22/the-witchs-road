using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 200f;
    public float distance = 4f;
    public float height = 2f;

    private float _rotX, _rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        _rotY += mouseDelta.x * sensitivity * Time.deltaTime;
        _rotX -= mouseDelta.y * sensitivity * Time.deltaTime;
        _rotX = Mathf.Clamp(_rotX, -30f, 60f);

        target.rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position
            - Quaternion.Euler(_rotX, _rotY, 0) * Vector3.forward * distance
            + Vector3.up * height;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }

    public Vector3 GetForwardDirection()
    {
        return transform.forward;
    }
}