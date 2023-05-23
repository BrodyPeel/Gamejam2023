using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform ship;
    public float smoothSpeed = 0.125f;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - ship.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(ship.position.x, ship.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition + offset, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
