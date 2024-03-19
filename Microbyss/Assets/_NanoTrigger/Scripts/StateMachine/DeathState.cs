using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{
    public string Name => "DeathState";

    public void OnEnter(StateController sc)
    {
        // "What was that!?"
        MenuController.Instance.screenFader.FadeToBlack();
        MenuController.Instance.ShowMenu(MenuController.Instance.resultsMenu);
        MenuController.Instance.eventSystem.SetSelectedGameObject(MenuController.Instance.resultsMenu.nextButton);
    }
    public void UpdateState(StateController sc)
    {
        // Search for player
    }
    public void OnPause(StateController sc)
    {
        // Transition to Pause State
        sc.ChangeState(sc.pauseState);
    }
    public void OnExit(StateController sc)
    {
        // "Must've been the wind"
    }
}
