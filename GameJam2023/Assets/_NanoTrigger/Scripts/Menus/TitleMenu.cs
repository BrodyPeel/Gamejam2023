using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : Menu
{

    public void Play()
    {
        // Open the level select menu
        Debug.Log("Play Button Pressed");
        AudioController.Instance.PlaySFX(SFX.Select);
        MenuController.Instance.ShowMenu(1);
        GameManager.Instance.state.ChangeState(GameManager.Instance.state.enterLevelState);
        Close();
    }

    public void Options()
    {
        // Open the options menu
        MenuController.Instance.ShowMenu(2);
        Close();
    }

    public void Quit()
    {
        // Quit the game
        AudioController.Instance.PlaySFX(SFX.Select);
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
