using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    new private Rigidbody2D rigidbody;

    public GameObject[] guns;

    public float boostSpeed;
    public float[] moveSpeed;
    public float[] fireRate;
    public float rotationSpeed = 5f;    
    public float rotationThreshold = 0.2f;

    public bool isBoosting = false;
    private bool isFiring = false;
    private float nextFireTime = 0f;
    private float nextDamageTime = 0.0f;
    private float damageInterval = 0.2f;
    private Vector2 joystickInput = Vector2.zero; 
    private Vector2 previousJoystickInput = Vector2.zero; 

    private int initialUpgradeLevel; // Store initial upgrade level here.
    public int upgradeLevel;
    public float maxLife;
    public float life;
    public int bombs = 3;
    private float bombInterval = 10.0f;
    private float nextBombFIre;

    public float LifePercentage
    {
        get
        {
            // Compute percentage of life remaining including resets.
            float totalLife = (life + ((upgradeLevel - 1) * 3)); // Total life left considering resets
            return (totalLife / maxLife) * 100;
        }
    }

    public SpriteRenderer[] spriteRenderers;
    public float flashDuration = 0.3f;
    public float fadeDuration = 1f;
    private float boostGlowAmount = 0f; 
    public float glowChangeSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        initialUpgradeLevel = upgradeLevel; // Store initial upgrade level.
        maxLife = initialUpgradeLevel * 3; // Calculate max life here.
        life = 3;
        bombs = 3;
        nextDamageTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            if (isFiring && Time.time >= nextFireTime)
            {
                FireWeapon();
                nextFireTime = Time.time + fireRate[upgradeLevel - 1];
            }

            if (upgradeLevel > 1 && life == 0)
            {
                upgradeLevel--;
                life = 3;
            }
            else if (upgradeLevel <= 1 && life <= 0)
            {
                Death();
            }


            if (isBoosting)
            {
                boostGlowAmount = Mathf.Min(boostGlowAmount + glowChangeSpeed * Time.deltaTime, 1f); // Ensure alpha doesn't go over 1
            }
            else
            {
                boostGlowAmount = Mathf.Max(boostGlowAmount - glowChangeSpeed * Time.deltaTime, 0f); // Ensure alpha doesn't go below 0
            }

            /*
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                float currentAlpha = sr.material.GetFloat("_AlphaTintFade");

                if (!Mathf.Approximately(currentAlpha, boostGlowAmount)) // Only update the alpha if it has changed
                {
                    sr.material.SetFloat("_AlphaTintFade", boostGlowAmount);
                }
            }
            */
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            Vector2 movementInput = context.ReadValue<Vector2>();
            float speed = isBoosting ? moveSpeed[upgradeLevel - 1] + boostSpeed : moveSpeed[upgradeLevel - 1];


            // Add force for movement
            rigidbody.AddForce(movementInput * speed, ForceMode2D.Force);

            // Check if there is no significant aiming input
            if (joystickInput.magnitude <= rotationThreshold)
            {
                // Use movement input for rotation if there's significant movement
                if (movementInput.magnitude > rotationThreshold)
                {
                    RotatePlayer(movementInput);
                }
            }
        }
    }


    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            joystickInput = context.ReadValue<Vector2>();
            UpdateAimDirection(joystickInput);
            // Rotate the player
            RotatePlayer(joystickInput);

            if (joystickInput.magnitude > 0f)
            {
                isFiring = true;
            }
            else
            {
                isFiring = false;
            }
        }
    }

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {

        }
    }

    public void OnUltFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            //implement 'ultimate' attack that kills everything
            if(bombs > 0 && Time.time > nextBombFIre)
            {
                //fire bomb
                nextBombFIre = Time.time + bombInterval; //cooldown between firing bomb
                //animate bomb cooldown?
                bombs--; //decrement bomb count
            }
            else if(bombs <= 0)
            {
                bombs = 0;
                //out of ammo message?
            }            
        }
    }

    void UpdateAimDirection(Vector2 aimInput)
    {
        GameManager.Instance.camera.aimDirection = aimInput.normalized; // Update the camera with the current aim direction
    }


    void RotatePlayer(Vector2 joystickInput)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            // Reset Angular Velocity to prevent unwanted drift
            rigidbody.angularVelocity = 0f;

            // Calculate the difference in direction from the last frame
            float directionChange = Vector2.Distance(joystickInput, previousJoystickInput);

            // Check if the joystick input magnitude is greater than the rotationThreshold 
            // and if the direction change is significant
            if (joystickInput.magnitude > rotationThreshold && directionChange > rotationThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, joystickInput);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Update previousJoystickInput for the next frame
            previousJoystickInput = joystickInput;
        }
    }

    void FireWeapon()
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            //pull bullet from object pool based on upgrade level
            //spawn the bullet at the appropriate gun position/rotation
            int numberOfBulletsToFire = upgradeLevel;

            if (upgradeLevel < 4)
            {
                for (int i = 0; i < numberOfBulletsToFire; i++)
                {
                    GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
                    if (bullet != null)
                    {
                        bullet.transform.position = guns[i].transform.position;
                        bullet.transform.rotation = guns[i].transform.rotation;
                        bullet.SetActive(true);
                        AudioController.Instance.PlaySFX(SFX.Fire1);
                    }
                }
            }
            else if (upgradeLevel == 4)
            {
                for (int i = 0; i < numberOfBulletsToFire; i++)
                {
                    GameObject bullet = ObjectPool.SharedInstance.GetPooledLargeObject();
                    if (bullet != null)
                    {
                        bullet.transform.position = guns[i].transform.position;
                        bullet.transform.rotation = guns[i].transform.rotation;
                        bullet.SetActive(true);
                        AudioController.Instance.PlaySFX(SFX.Fire1);
                    }
                }
            }
        }
    } 
    
    public void OnBoost(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            if (context.started)
            {
                isBoosting = true;
            }
            else if (context.canceled)
            {
                isBoosting = false;
            }
        }
    }

    public void Damage(float Damage)
    {
        if (Time.time > nextDamageTime)
        {
            life -= Damage;
            nextDamageTime += Time.time + damageInterval;
            if (life <= 0)
            {
                life = 0;
            }

            if (Damage > 0) // If the player takes damage, start the flash.
            {
                StartCoroutine(FlashPlayer());
            }
        }
    }

    private IEnumerator FlashPlayer()
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


    public void Death()
    {
        //death animation
        //results screen
        Die();
        //Destroy(this.gameObject);
    }

    public void Die()
    {
        StartCoroutine(_Die(0f));
    }

    private IEnumerator _Die(float targetAlpha)
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

        GameManager.Instance.state.ChangeState(GameManager.Instance.state.resultState);
    }
}



