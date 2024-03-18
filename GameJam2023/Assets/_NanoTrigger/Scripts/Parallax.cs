using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffectMultiplier = 0.5f;

    private Transform cameraTransform;
    private Vector3 startPosition;
    private float startZ;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    private void Update()
    {
        // Calculate the distance the camera has moved since the start
        Vector3 cameraDistance = cameraTransform.position - startPosition;

        // Apply the parallax effect based on that distance
        // Notice that we're ignoring the Z distance since parallax is usually a 2D effect
        float parallax = cameraDistance.x * parallaxEffectMultiplier;
        float parallaxY = cameraDistance.y * parallaxEffectMultiplier;

        // Set the position based on the original position plus the calculated parallax
        // and ensure we maintain the original Z position to keep layering/ordering
        transform.position = new Vector3(startPosition.x + parallax, startPosition.y + parallaxY, startZ);
    }
}
