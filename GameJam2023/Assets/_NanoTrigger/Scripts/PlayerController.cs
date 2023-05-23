using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;

    public GameObject leftGun;
    public GameObject rightGun;
    public GameObject firstBullet;
    public float moveSpeed;

    public float rotationSpeed = 5f;
    public float fireRate = 0.2f;

    private bool isFiring = false;
    private float nextFireTime = 0f;
    private Vector2 joystickInput = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody = this.GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Read value from control. The type depends on what type of controls.
        // the action is bound to.        
        Vector2 v = context.ReadValue<Vector2>();
        rigidbody.AddForce(v*moveSpeed, ForceMode2D.Force);
        
        // IMPORTANT:
        // The given InputValue is only valid for the duration of the callback. Storing the InputValue references somewhere and calling Get<T>() later does not work correctly.
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        joystickInput = context.ReadValue<Vector2>();

        // Rotate the player
        RotatePlayer();

        // Handle firing
        if (joystickInput.magnitude > 0f)
        {
            if (!isFiring)
            {
                isFiring = true;
                nextFireTime = Time.time;
            }

            // Check if enough time has passed to fire
            if (Time.time >= nextFireTime)
            {
                FireWeapon();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            isFiring = false;
        }
    }

    void RotatePlayer()
    {
        // Rotate the player based on joystick input
        if (joystickInput.magnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, joystickInput);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FireWeapon()
    {
        //check for upgrade level? 

        // Instantiate left bullet
        GameObject leftBullet = Instantiate(firstBullet, leftGun.transform.position, Quaternion.identity);
        leftBullet.transform.rotation = transform.rotation;

        // Instantiate right bullet
        GameObject rightBullet = Instantiate(firstBullet, rightGun.transform.position, Quaternion.identity);
        rightBullet.transform.rotation = transform.rotation;
    }
}
    
    

