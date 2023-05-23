using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxFactorX = 0.5f;
    [SerializeField] private float parallaxFactorY = 0.5f;

    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
    }

    private void Update()
    {
        // Calculate the parallax effect
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxFactorX, deltaMovement.y * parallaxFactorY, 0);
        _lastCameraPosition = _cameraTransform.position;
    }
}
