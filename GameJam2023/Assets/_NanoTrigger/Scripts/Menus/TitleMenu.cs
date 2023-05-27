using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : Menu
{
    public void Play()
    {
        // Open the level select menu
        Debug.Log("Play Button Pressed");
        AudioController.Instance.PlaySFX(SFX.Select);
        GameManager.Instance.state.ChangeState(GameManager.Instance.state.enterLevelState);
        Close();
    }
}
