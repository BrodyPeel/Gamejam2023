using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, 
    Chase,
    Attack,
    Death
}

public class Enemy : MonoBehaviour
{
    protected delegate void enemyDeathHandler(bool x);
    protected event enemyDeathHandler OnEnemyDeath;

    protected EnemyState currentState;
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float sightRadius;
    [SerializeField]
    protected float attackRadius;
    [SerializeField]
    protected float rotationSpeed;
    [SerializeField]
    protected bool canMove = false;
    [SerializeField]
    protected float damage;

    public float fadeDuration = 1f;
    private SpriteRenderer[] spriteRenderers;
    

    protected Vector2 enemyPosition;
    protected Transform PlayerPosition;
    protected Vector2 playerShipTransform;

    public float flashDuration = 0.3f;

    protected bool dead = false;

    // Start is called before the first frame update
    // Start is called before the first frame update
    public virtual void Start()
    {
        currentState = EnemyState.Idle;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.material.SetFloat("_FullAlphaDissolveFade", 1f);
        }

        // Find the player's transform at the start to avoid repeated searches.
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if (dead || PlayerPosition == null) return;

        // Check the distance to the player and proceed only if within sightRadius
        if (Vector2.Distance(transform.position, PlayerPosition.position) > sightRadius)
        {
            return; // Player is too far, skip the rest of the update
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                // Transition to Chase or Attack based on player's proximity
                break;
            case EnemyState.Chase:
                // Chase player
                break;
            case EnemyState.Attack:
                // Attack player
                break;
            case EnemyState.Death:
                // Handle death
                break;
        }

        if (health <= 0)
        {
            dead = true;
            Death();
        }

        foreach (Transform child in transform)
        {
            var tracker = child.GetComponent<CollisionTracker>();
            if (tracker != null)
            {
                foreach (var collision in tracker.collisions)
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        collision.gameObject.GetComponent<PlayerController>().Damage(damage);
                    }
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        //animation for damage?
        AudioController.Instance.PlaySFX(SFX.EnemySpawn1);
        health -= damage;
        StartCoroutine(FlashEnemy());
    }

    private IEnumerator FlashEnemy()
    {
        float startTime = Time.time; // Save the start time.

        while (Time.time < startTime + flashDuration)
        {
            // The 2nd parameter to PingPong determines how fast it oscillates
            // We divide flashDuration by 2 so that it goes from 0 to 1 and back to 0 over the course of flashDuration
            float pingPongValue = Mathf.PingPong((Time.time - startTime) / (flashDuration / 2), 1);

            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.material.SetFloat("_AddColorFade", pingPongValue);
            }

            yield return null; // Yield to the next frame.
        }

        // Ensure all sprites return to their original state after the loop
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.material.SetFloat("_AddColorFade", 0);
        }
    }

    public virtual void Death()
    {
        //GameManager.Instance.levelManager.currentLevel.cleared += 1;
        AudioController.Instance.PlaySFX(SFX.EnemyDie1);
        Fade();
        OnEnemyDeath?.Invoke(false);
    }

    public void Spawn()
    {
        OnEnemyDeath?.Invoke(true);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Damage(damage);
        }
    }

    public void Fade()
    {
        StartCoroutine(FadeTo(0f));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Dictionary<SpriteRenderer, float> startAlphas = new Dictionary<SpriteRenderer, float>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            startAlphas.Add(sr, sr.material.GetFloat("_FullGlowDissolveFade"));
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            foreach (SpriteRenderer sr in spriteRenderers)
            {
                float alpha = Mathf.Lerp(startAlphas[sr], targetAlpha, elapsedTime / fadeDuration);
                sr.material.SetFloat("_FullGlowDissolveFade", alpha);
            }

            yield return null;
        }

        Destroy(this.gameObject);
    }

}
