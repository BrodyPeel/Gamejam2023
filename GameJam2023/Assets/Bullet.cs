using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D RB;

    private float deactivateTime = 3.0f;

    public float force;
    // Start is called before the first frame update
    
    void OnEnable()
    {
        RB = this.GetComponent<Rigidbody2D>(); 
        RB.AddForce(transform.up * force, ForceMode2D.Impulse);

        //deactivate after 5 seconds for testing.
        Invoke("DeactivateBullet", deactivateTime);
    }

    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }


}
