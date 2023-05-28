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
    private SpriteRenderer[] sprites;
    

    protected Vector2 enemyPosition;
    protected Transform PlayerPosition;
    protected Vector2 playerShipTransform;

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentState = EnemyState.Idle;
        GameManager.Instance.levelManager.currentLevel.clearCount += 1;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in sprites)
        {
            sr.material.SetFloat("_FullAlphaDissolveFade", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        switch (currentState)
        {
            case EnemyState.Idle:
                // Handle idle state behavior
                //if see the nanobot currentState = EnemyState.Chase
                //if nanobot within attackradius currentState = EnemyState.Attack.
                break;
            case EnemyState.Chase:
                // Handle chase state behavior
                break;
            case EnemyState.Attack:
                // Handle attack state behavior
                break;
            case EnemyState.Death:
                //handle death behavior
                break;
        }

        if(health <= 0)
        {
            Death();
            Destroy(this.gameObject);
        }

    }

    public virtual void TakeDamage(float damage)
    {
        //animation for damage?
        AudioController.Instance.PlaySFX(SFX.EnemySpawn1);
        health -= damage;
    }

    public void Death()
    {
        GameManager.Instance.levelManager.currentLevel.cleared += 1;
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
        foreach (SpriteRenderer sr in sprites)
        {
            float startAlpha = sr.material.GetFloat("_FullGlowDissolveFade");
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                sr.material.SetFloat("_FullGlowDissolveFade", alpha);
                yield return null;
            }
        }
    }
}
