using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D RB;

    public float force;
    // Start is called before the first frame update

    private void Start()
    {
        
    }
    void OnEnable()
    {
        RB = this.GetComponent<Rigidbody2D>(); 
        RB.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
