using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public bool isBoss = false;

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
    public GameObject cameraContainer;  // The container for the camera during the door animation
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
                if (!isBoss)
                {
                    StartDoorAnimation();
                }
                else
                {
                    GameManager.Instance.levelManager.CompleteLevel();
                    levelStarted = false;
                }
            }
        }
    }

    public void StartLevelAnimation()
    {
        playerShip = GameManager.Instance.ship;

        // Unparent the ship
        playerShip.transform.SetParent(GameManager.Instance.gameObject.transform);

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
        playerShip.transform.SetParent(GameManager.Instance.gameObject.transform);

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
        playerShip.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerShip.GetComponent<Rigidbody2D>().angularVelocity = 0f;

        // Parent the ship to the container
        GameManager.Instance.camera.target = cameraContainer.transform;

        GameManager.Instance.state.ChangeState(GameManager.Instance.state.exitLevelState);

        StartCoroutine(ShipExit());
    }

    private IEnumerator ShipExit()
    {
        MenuController.Instance.screenFader.FadeToBlack();

        while (true)
        {
            // Move towards the target position smoothly
            playerShip.transform.position = Vector2.MoveTowards(playerShip.transform.position, exitPosition.position, 2f * Time.deltaTime);

            // Check if the object has reached the target

            if (Vector3.Distance(playerShip.transform.position, exitPosition.position) <= 0.1f)
            {
                Debug.Log("Exit Animation");
                animator.Play(exitAnimation);


                GameManager.Instance.levelManager.CompleteLevel();
                GameManager.Instance.camera.target = GameManager.Instance.ship.transform;
                Destroy(this.gameObject);
                break;
            }

            // Return control to Unity and wait until the next frame to continue execution
            yield return null;
        }
    }

    public void ExitAnimationFinished()
    {

    }
}
