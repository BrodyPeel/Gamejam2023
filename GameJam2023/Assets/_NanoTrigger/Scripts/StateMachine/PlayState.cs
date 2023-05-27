using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : IState
{
    public string Name => "PlayState";

    public void OnEnter(StateController sc)
    {
        MenuController.Instance.ShowMenu(1);
        Time.timeScale = 1f;
    }
    public void UpdateState(StateController sc)
    {
        GameManager.Instance.playtime += Time.deltaTime;
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
