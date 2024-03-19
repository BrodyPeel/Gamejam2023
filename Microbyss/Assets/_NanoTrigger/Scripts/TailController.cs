using System.Collections;
using UnityEngine;

public class TailController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private int lengthOfLineRenderer = 20;
    [SerializeField] private float tailSegmentSpacing = 0.1f;
    [SerializeField] private float waveMagnitudeIdle = 0.5f;
    [SerializeField] private float waveMagnitudeMoving = 0.2f;
    [SerializeField] private float waveFrequencyIdle = 1f;
    [SerializeField] private float waveFrequencyMoving = 3f;
    [SerializeField] private float maxTransitionSpeed = 10f;
    private Vector3[] tailPositions;
    private float phaseOffset; // Unique phase offset for this tail

    void Start()
    {
        lineRenderer.positionCount = lengthOfLineRenderer;
        tailPositions = new Vector3[lengthOfLineRenderer];
        InitializeTailPositions();
        // Adding a slight delay might help in reducing the chance of identical phase offsets
        StartCoroutine(AssignPhaseOffsetWithDelay());
    }

    IEnumerator AssignPhaseOffsetWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.1f)); // Short delay
        phaseOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        float velocityMagnitude = rigidbody.velocity.magnitude;
        UpdateTailBasedOnVelocity(velocityMagnitude);
    }

    void InitializeTailPositions()
    {
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            tailPositions[i] = playerTransform.position - playerTransform.up * tailSegmentSpacing * i;
        }
    }

    void UpdateTailBasedOnVelocity(float velocityMagnitude)
    {
        float waveMagnitude = Mathf.Lerp(waveMagnitudeIdle, waveMagnitudeMoving, velocityMagnitude / 10f);
        float waveFrequency = Mathf.Lerp(waveFrequencyIdle, waveFrequencyMoving, velocityMagnitude / 10f);
        float transitionSpeed = Mathf.Lerp(1f, maxTransitionSpeed, velocityMagnitude / 10f);

        tailPositions[0] = playerTransform.position;
        lineRenderer.SetPosition(0, tailPositions[0]);

        for (int i = 1; i < lengthOfLineRenderer; i++)
        {
            float phase = Time.time * waveFrequency + i + phaseOffset; // Use phaseOffset here
            float wave = Mathf.Sin(phase) * waveMagnitude;

            Vector3 direction = (tailPositions[i - 1] - playerTransform.position).normalized;
            Vector3 waveDirection = Quaternion.Euler(0, 0, 90) * direction;
            Vector3 targetPosition = tailPositions[i - 1] + direction * tailSegmentSpacing + waveDirection * wave;

            tailPositions[i] = Vector3.Lerp(tailPositions[i], targetPosition, Time.deltaTime * transitionSpeed);
            lineRenderer.SetPosition(i, tailPositions[i]);
        }
    }
}
