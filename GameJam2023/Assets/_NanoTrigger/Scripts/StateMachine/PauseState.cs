using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IState
{
    public void OnEnter(StateController sc)
    {
        // "What was that!?"
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
