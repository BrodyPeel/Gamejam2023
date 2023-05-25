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
    protected EnemyState currentState;
    protected float health;
    protected float moveSpeed;
    protected float sightRadius;



    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Idle;
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
    }
}
