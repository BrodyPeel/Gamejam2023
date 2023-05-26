using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleState : IState
{
    public void OnEnter(StateController sc)
    {
        // "What was that!?"
        MenuController.Instance.screenFader.FadeFromBlack();
        GameManager.Instance.camera.AdjustSizeOverTime(6f, 0f);
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
