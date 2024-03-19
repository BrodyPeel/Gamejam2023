using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevelState : IState
{
    public string Name => "EnterLevelState";

    public void OnEnter(StateController sc)
    {
        GameManager.Instance.levelManager.currentLevel.StartLevelAnimation();
        MenuController.Instance.screenFader.FadeFromBlack();
    }
    public void UpdateState(StateController sc)
    {
    }
    public void OnPause(StateController sc)
    {
        // Transition to Pause State
        sc.ChangeState(sc.pauseState);
    }
    public void OnExit(StateController sc)
    {
    }
}
