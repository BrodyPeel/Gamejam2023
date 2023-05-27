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

    public int upgradeLevel;
    public float life;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();       
        
    }

    // Update is called once per frame
    void Update()
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

    public void OnMove(InputAction.CallbackContext context)
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

    public void OnPrimaryFire(InputAction.CallbackContext context)
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

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {

    }

    public void OnUltFire(InputAction.CallbackContext context)
    {
        //implement 'ultimate' attack that kills everything
        //limited fire
    }

    void RotatePlayer(Vector2 joystickInput)
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

    void FireWeapon()
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
    
    public void OnBoost(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isBoosting = true;
        }
        else if (context.canceled)
        {
            isBoosting = false;
        }
    }

    public void Damage(float Damage)
    {
        life -= Damage;
        if(life <= 0)
        {
            life = 0;
        }
    }

    public void Death()
    {
        //death animation
        //results screen
        Destroy(this.gameObject);
    }   

}
    
    

