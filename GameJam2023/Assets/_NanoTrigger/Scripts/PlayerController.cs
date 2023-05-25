using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;

    public GameObject[] guns;
    public GameObject firstBullet;

    public float[] moveSpeed;
    public float rotationSpeed = 5f;
    public float[] fireRate;

    private bool isFiring = false;
    private float nextFireTime = 0f;
    private Vector2 joystickInput = Vector2.zero;

    private int upgradeLevel;
    private float life;

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
        rigidbody.AddForce(v * moveSpeed[upgradeLevel], ForceMode2D.Force);        
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
                nextFireTime = Time.time + fireRate[upgradeLevel];
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FireWeapon()
    {
        //pull bullet from object pool based on upgrade level
        //spawn the bullet at the appropriate gun position/rotation
        int numberOfBulletsToFire = upgradeLevel;

        for (int i = 0; i < numberOfBulletsToFire; i++)
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = guns[i].transform.position;
                bullet.transform.rotation = guns[i].transform.rotation;
                bullet.SetActive(true);
            }
        }


    }    
    
}
    
    

