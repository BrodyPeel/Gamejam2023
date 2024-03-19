using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class UnderwaterEffectController : MonoBehaviour
{
    public Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody
    private ParticleSystem particleSystem;

    public float minEmissionRate = 0f; // Minimum emission rate when player is idle
    public float maxEmissionRate = 50f; // Maximum emission rate at or above maxVelocity
    public float maxVelocity = 5f; // Velocity at which emission rate is maxed

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = minEmissionRate; // Initially set to minimum
    }

    void Update()
    {
        var emissionModule = particleSystem.emission;
        float velocityMagnitude = playerRigidbody.velocity.magnitude;

        // Map the player's velocity to the emission rate linearly
        float emissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, velocityMagnitude / maxVelocity);
        emissionModule.rateOverTime = emissionRate;
    }
}
