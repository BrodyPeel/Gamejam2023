using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class UnderwaterTrail : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    private TrailRenderer trailRenderer;

    public float minWidth = 0.1f;
    public float maxWidth = 0.5f;
    public float maxSpeed = 5f; // Adjust based on your player's max speed

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        // Calculate the width based on the current speed
        float speed = playerRigidbody.velocity.magnitude;
        float width = Mathf.Lerp(minWidth, maxWidth, speed / maxSpeed);

        // Apply the calculated width to the trail
        trailRenderer.startWidth = width;
        trailRenderer.endWidth = width * 0.5f; // End width is half of the start width
    }
}
