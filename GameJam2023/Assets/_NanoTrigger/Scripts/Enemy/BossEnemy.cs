using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    Rigidbody2D rb;

    public float attackInterval;
    public float nextFire;
    public float intervalModifer;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Idle;

        rb = this.GetComponent<Rigidbody2D>();

        GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
        if (playerShip != null)
        {
            PlayerPosition = playerShip.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerShipTransform = PlayerPosition.transform.position;
        enemyPosition = this.transform.position;

        //health less than 0. move to death state
        if (health <= 0.0f)
        {
            currentState = EnemyState.Death;
        }

        if(GameManager.Instance.state.isState("PlayState"))
        {
            //check to see where player is. Change state accordingly            
            if (Vector2.Distance(playerShipTransform, enemyPosition) <= attackRadius)
            {
                currentState = EnemyState.Attack;
            }
        }

        if (currentState == EnemyState.Attack)
        {            
            Attack();

            if(Time.time >= nextFire)
            {
                Projectile(Random.Range(0, 3));
                nextFire = Time.time + attackInterval;
            }
        }
    }

    public void Attack()
    {

    }

    public void Projectile(int Sequence)
    {
        
        if (Sequence == 1) //fires bullets 6 bullets in a spread
        {
            float spreadAngle = 30f;
            int numberOfProjectiles = 6;

            float angleIncrement = spreadAngle / (numberOfProjectiles - 1);
            float startAngle = -spreadAngle / 2f;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                GameObject bullet = ObjectPool.SharedInstance.GetPooledEnemyProjectileOne();
                float angle = startAngle + i * angleIncrement;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);                

                if (bullet != null)
                {
                    bullet.transform.position = this.transform.position;
                    bullet.transform.rotation = this.transform.rotation * rotation;
                    bullet.SetActive(true);
                    //TODO replace with different sound
                    //AudioController.Instance.PlaySFX(SFX.Fire1);
                }
            }
            
        }
        else if(Sequence == 2) //Fires 6 bullets in a line
        {
            Vector2 bossPosition = transform.position;
            Vector2 playerDirection = playerShipTransform - bossPosition;
            Vector2 normalizedDirection = playerDirection.normalized;

            for (int i = 0; i < 6; i++)
            {
                GameObject bullet = ObjectPool.SharedInstance.GetPooledEnemyProjectileOne();
                if (bullet != null)
                {
                    bullet.transform.position = bossPosition; // Set the initial position of the projectile to the boss position
                    bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, normalizedDirection); // Set the rotation of the projectile to face the player
                    bullet.SetActive(true);
                    //TODO replace with different sound
                    //AudioController.Instance.PlaySFX(SFX.Fire1);
                }
            }
        }
        else if (Sequence == 3) // fires 12 projectiles in larger spread. Random location.
        {
            float spreadAngle = 90f;
            int numberOfProjectiles = 12;

            float angleIncrement = spreadAngle / (numberOfProjectiles - 1);
            float startAngle = -spreadAngle / 2f;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                GameObject bullet = ObjectPool.SharedInstance.GetPooledEnemyProjectileOne();
                float angle = startAngle + (Random.Range(1, numberOfProjectiles)) * angleIncrement;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                if (bullet != null)
                {
                    bullet.transform.position = this.transform.position;
                    bullet.transform.rotation = this.transform.rotation * rotation;
                    bullet.SetActive(true);
                    //TODO replace with different sound
                    //AudioController.Instance.PlaySFX(SFX.Fire1);
                }
            }
        }
        
        
    }

    
}
