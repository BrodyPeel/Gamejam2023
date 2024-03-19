using System.Collections;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    private Material material;
    private Vector3 lastPosition;
    private Vector2 totalDistanceTravelled;
    public float scaleFactor;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate the distance travelled since the last frame
        Vector3 distanceTravelled = transform.position - lastPosition;
        // Update the last position for the next frame
        lastPosition = transform.position;

        // Accumulate the total distance travelled in 2D by adding the distance travelled this frame
        totalDistanceTravelled += new Vector2(distanceTravelled.x, distanceTravelled.y);

        // Adjust the texture offset based on the total distance travelled.
        // You might need to adjust the scale factor to get the desired texture scroll speed.
        Vector2 textureOffset = totalDistanceTravelled * scaleFactor; // Scale factor for adjusting scroll speed

        // Apply the calculated texture offset
        material.SetVector("_ScrollOffset", textureOffset);

        // Lock Z rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
    }
}