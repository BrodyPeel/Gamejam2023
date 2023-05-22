using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;

    public float MoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody = this.GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputValue value)
    {
        // Read value from control. The type depends on what type of controls.
        // the action is bound to.
        Debug.Log("move");
        Vector2 v = value.Get<Vector2>();
        rigidbody.AddForce(v*MoveSpeed, ForceMode2D.Force);        
        // IMPORTANT:
        // The given InputValue is only valid for the duration of the callback. Storing the InputValue references somewhere and calling Get<T>() later does not work correctly.
    }
}
