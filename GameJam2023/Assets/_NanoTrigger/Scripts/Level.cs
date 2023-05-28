using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    public int clearCount;

    public int cleared = 0;

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
    private GameObject shipContainer;  // The container for the ship during the opening animation
    [SerializeField]
    private GameObject shipExitContainer;  // The container for the ship during the exit animation
    [SerializeField]
    private GameObject cameraContainer;  // The container for the camera during the door animation
    [SerializeField]
    private Animator animator;

    private string openingAnimation = "EnterLevel";
    private string doorAnimation = "DoorAnimation";
    private string exitAnimation = "ExitLevel";

    public Transform exitPosition;


    private bool levelStarted = false;
    private bool doorOpened = false;

    public ExitTrigger exitTrigger;

    private void Awake()
    {
        exitTrigger.level = this;
    }

    private void Update()
    {
        if (levelStarted)
        {
            if(cleared >= clearCount)
            {
                // Open Door
                Debug.Log("Level Complete");
                StartDoorAnimation();
            }
        }
    }

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

    public void EnterAnimationFinished()
    {
        // Unparent the ship
        playerShip.transform.SetParent(null);

        // Set ship rigidbody to Dynamic
        playerShip.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Zoom in camera
        GameManager.Instance.camera.AdjustSizeOverTime(4f, 3f);

        GameManager.Instance.state.ChangeState(GameManager.Instance.state.playState);

        levelStarted = true;
    }

    public void StartDoorAnimation()
    {
        if (doorOpened) return;
        doorOpened = true;
        playerShip = GameManager.Instance.ship;

        // Parent the ship to the container
        GameManager.Instance.camera.target = cameraContainer.transform;

        // Start the animation
        animator.Play(doorAnimation);
    }

    public void DoorAnimationFinished()
    {
        GameManager.Instance.camera.target = GameManager.Instance.ship.transform;
    }

    public void StartExitAnimation()
    {
        // Set ship rigidbody to Kinematic
        playerShip.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // Parent the ship to the container
        GameManager.Instance.camera.target = cameraContainer.transform;

        ShipExit();
    }

    private IEnumerator ShipExit()
    {
        while (true)
        {
            // Calculate the rotation we need to target
            Quaternion targetRotation = Quaternion.LookRotation(playerShip.transform.position - exitPosition.position);

            // Slerp towards the target rotation smoothly
            playerShip.transform.rotation = Quaternion.Slerp(playerShip.transform.rotation, targetRotation, 0.1f * Time.deltaTime);
            // Check if the object has reached the target

            if (Vector3.Distance(playerShip.transform.position, exitPosition.position) <= 0.1f)
            {
                break;
            }

            // Return control to Unity and wait until the next frame to continue execution
            yield return null;
        }
    }

    public void ExitAnimationFinished()
    {
        // Set ship rigidbody to Dynamic
        playerShip.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
