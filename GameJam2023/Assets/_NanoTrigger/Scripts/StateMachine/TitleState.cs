using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleState : IState
{
    public string Name => "TitleState";

    public void OnEnter(StateController sc)
    {
        MenuController.Instance.screenFader.FadeFromBlack();
        GameManager.Instance.camera.AdjustSizeOverTime(4f, 0f);
        AudioController.Instance.PlayMusic(Music.NTLoop, true);
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
