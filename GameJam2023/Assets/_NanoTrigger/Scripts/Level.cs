using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private int clearCount;

    public int ClearCount
    {
        get { return clearCount; }
        private set { clearCount = value; }
    }

    [SerializeField]
    private int depth;

    public int Depth
    {
        get { return depth; }
        private set { depth = value; }
    }

    [SerializeField]
    private GameObject playerShip;
    [SerializeField]
    private GameObject shipContainer;  // The container for the ship during the animation
    [SerializeField]
    private Animator animator;

    private string openingAnimation = "EnterLevel";


    public void StartLevelAnimation()
    {
        playerShip = GameManager.Instance.ship;

        // Set ship rigidbody to Kinematic
        playerShip.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // Parent the ship to the container
        playerShip.transform.SetParent(shipContainer.transform);
        playerShip.transform.position = shipContainer.transform.position;
        playerShip.transform.rotation = shipContainer.transform.rotation;

        // Start the animation
        animator.Play(openingAnimation);
    }

    public void AnimationFinished()
    {
        // Unparent the ship
        playerShip.transform.SetParent(null);

        // Set ship rigidbody to Dynamic
        playerShip.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Zoom in camera
        GameManager.Instance.camera.AdjustSizeOverTime(4f, 3f);

    }
}
