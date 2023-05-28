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

    private bool isBoosting = false;
    private bool isFiring = false;
    private float nextFireTime = 0f;
    private Vector2 joystickInput = Vector2.zero;

    private int initialUpgradeLevel; // Store initial upgrade level here.
    public int upgradeLevel;
    public float maxLife;
    public float life;

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

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        initialUpgradeLevel = upgradeLevel; // Store initial upgrade level.
        maxLife = initialUpgradeLevel * 3; // Calculate max life here.
        life = 3;
    }

    // Update is called once per frame
    void Update()
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
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            Vector2 leftStickInput = context.ReadValue<Vector2>();
            float speed = isBoosting ? moveSpeed[upgradeLevel - 1] + boostSpeed : moveSpeed[upgradeLevel - 1];
            // Check if the right stick is not in use
            if (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude <= 0f)
            {
                // Call RotatePlayer() only when the left stick is used and the right stick is not in use
                if (leftStickInput.magnitude > 0f)
                {
                    RotatePlayer(leftStickInput);
                }
            }

            rigidbody.AddForce(leftStickInput * speed, ForceMode2D.Force);
        }
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            joystickInput = context.ReadValue<Vector2>();

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
            //limited fire
        }
    }

    void RotatePlayer(Vector2 joystickInput)
    {
        if (GameManager.Instance.state.isState("PlayState"))
        {
            // Rotate the player based on joystick input
            if (joystickInput.magnitude > rotationThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, joystickInput);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else if (joystickInput.magnitude <= 0.0f)
            {

            }
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
        life -= Damage;
        if(life <= 0)
        {
            life = 0;
        }

        if (Damage > 0) // If the player takes damage, start the flash.
        {
            StartCoroutine(FlashPlayer());
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

        GameManager.Instance.state.ChangeState(GameManager.Instance.state.deathState);
    }
}



