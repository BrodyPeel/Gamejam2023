using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IState
{
    public string Name => "PauseState";

    public void OnEnter(StateController sc)
    {
        MenuController.Instance.ShowMenu(MenuController.Instance.pauseMenu);
        MenuController.Instance.eventSystem.SetSelectedGameObject(MenuController.Instance.pauseMenu.resumeButton);
        Time.timeScale = 0f;
    }

    public void UpdateState(StateController sc)
    {
        // Search for player
    }

    public void OnPause(StateController sc)
    {
        //sc.ChangeState(sc.playState);
    }

    public void OnExit(StateController sc)
    {
        // "Must've been the wind"
    }
}
