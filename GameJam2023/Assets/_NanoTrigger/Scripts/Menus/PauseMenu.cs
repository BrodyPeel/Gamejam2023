using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    public GameObject resumeButton;
    public GameObject quitButton;

    public void Resume()
    {
        // Open the level select menu
        Debug.Log("Resume Button Pressed");
        AudioController.Instance.PlaySFX(SFX.Select);
        MenuController.Instance.Pause();
    }

    public void Quit()
    {
        //Reset Player Position

        //Reset Game Stats

        //Reload first level

        // Open the title menu
        Debug.Log("Quit Button Pressed");
        AudioController.Instance.PlaySFX(SFX.Select);
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
