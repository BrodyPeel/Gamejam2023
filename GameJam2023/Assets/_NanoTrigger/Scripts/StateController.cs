using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    IState currentState;
    public TitleState titleState = new TitleState();
    public PlayState playState = new PlayState();
    public PauseState pauseState = new PauseState();
    public ResultState resultState = new ResultState();
    private void Start()
    {
        ChangeState(titleState);
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        currentState.OnEnter(this);
    }
}
public interface IState
{
    public void OnEnter(StateController controller);
    public void UpdateState(StateController controller);
    public void OnPause(StateController controller);
    public void OnExit(StateController controller);
}