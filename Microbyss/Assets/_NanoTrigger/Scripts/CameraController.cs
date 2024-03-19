using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player's transform
    public float smoothSpeed = 0.125f; // How smoothly the camera follows the target
    public Vector2 aimDirection; // The direction the player is aiming, to be updated from the player controller
    public float lookaheadDistance = 3.0f; // How far ahead of the player the camera should look based on the aim direction
    public float adjustSpeed = 2.0f; // Speed of camera adjustment to the new position

    void LateUpdate()
    {
        // Calculate the lookahead position based on the player's aim direction
        Vector3 lookahead = new Vector3(aimDirection.x, aimDirection.y, 0) * lookaheadDistance;

        // Calculate the target position for the camera by adding the lookahead to the player's position
        Vector3 targetPosition = target.position + lookahead;

        // Ensure the camera maintains its current z position
        targetPosition.z = transform.position.z;

        // Smoothly interpolate between the camera's current position and the target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime * adjustSpeed);
        transform.position = smoothedPosition;
    }

    // Method to adjust camera's orthographic size over time (remains unchanged)
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

        // Ensure the camera's size is exactly the target size at the end of duration
        GetComponent<Camera>().orthographicSize = targetSize;
    }
}
