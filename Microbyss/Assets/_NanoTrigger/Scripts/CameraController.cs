using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player's transform
    public Rigidbody2D targetRigidbody; // Player's Rigidbody for velocity checking
    public float followSmoothSpeed = 1.5f; // Smooth speed for general following
    public float aimSmoothSpeed = 0.5f; // Smooth speed for aiming
    private float currentSmoothSpeed; // Active smooth speed
    public Vector2 aimDirection; // The direction the player is aiming
    public float lookaheadDistance = 3.0f; // Lookahead distance based on aim direction
    public float adjustSpeed = 2.0f; // Speed of camera adjustment to new position

    public float minOrthographicSize = 5.0f; // Minimum orthographic size
    public float maxOrthographicSize = 10.0f; // Maximum orthographic size
    public float sizeAdjustmentSpeed = 0.5f; // How quickly the camera size adjusts

    private Camera cam; // Reference to the camera component

    void Start()
    {
        cam = GetComponent<Camera>();
        currentSmoothSpeed = followSmoothSpeed; // Initialize with general following smooth speed
    }

    void LateUpdate()
    {
        AdjustSmoothSpeed(); // Adjust smooth speed based on velocity and aiming

        Vector3 lookahead = new Vector3(aimDirection.x, aimDirection.y, 0) * lookaheadDistance;
        Vector3 targetPosition = target.position + lookahead;
        targetPosition.z = transform.position.z;

        // Use the dynamically adjusted smooth speed for interpolation
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, currentSmoothSpeed * Time.deltaTime * adjustSpeed);
        transform.position = smoothedPosition;

        AdjustOrthographicSize(); // Adjust camera's orthographic size
    }

    private void AdjustSmoothSpeed()
    {
        currentSmoothSpeed = aimDirection.magnitude > 0.1f ? aimSmoothSpeed : followSmoothSpeed;
        float velocityFactor = Mathf.Clamp(targetRigidbody.velocity.magnitude / 10.0f, 0.5f, 1.5f);
        currentSmoothSpeed *= velocityFactor;
    }

    private void AdjustOrthographicSize()
    {
        // Calculate the distance from the player to the center of the camera
        float distanceToCenter = Vector2.Distance(cam.WorldToViewportPoint(target.position), new Vector2(0.5f, 0.5f));

        // Map the distance to the orthographic size range
        float targetSize = Mathf.Lerp(minOrthographicSize, maxOrthographicSize, distanceToCenter);

        // Smoothly adjust the camera's orthographic size
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetSize, sizeAdjustmentSpeed * Time.deltaTime);
    }

    public void AdjustSizeOverTime(float targetSize, float duration)
    {
        StartCoroutine(ChangeSize(targetSize, duration));
    }

    private IEnumerator ChangeSize(float targetSize, float duration)
    {
        float startTime = Time.time;
        float initialSize = GetComponent<Camera>().orthographicSize;

        while (Time.time - startTime < duration)
        {
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(initialSize, targetSize, (Time.time - startTime) / duration);
            yield return null;
        }

        GetComponent<Camera>().orthographicSize = targetSize;
    }
}
