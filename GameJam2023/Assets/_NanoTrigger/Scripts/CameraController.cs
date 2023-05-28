using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    // Add this method to adjust orthographic size over time
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

        // Ensure the size is exactly the target size at the end of the duration
        GetComponent<Camera>().orthographicSize = targetSize;
    }
}