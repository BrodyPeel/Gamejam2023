using System.Collections.Generic;
using UnityEngine;

public class CollisionTracker : MonoBehaviour
{
    public List<Collision2D> collisions = new List<Collision2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Damage(1);
        }
        collisions.Add(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisions.Remove(collision);
    }
}
