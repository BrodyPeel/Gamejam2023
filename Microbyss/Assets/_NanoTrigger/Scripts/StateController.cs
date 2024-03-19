using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField]
    IState currentState;
    public TitleState titleState = new TitleState();
    public EnterLevelState enterLevelState = new EnterLevelState();
    public ExitLevelState exitLevelState = new ExitLevelState();
    public PlayState playState = new PlayState();
    public DeathState deathState = new DeathState();
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

        Debug.Log(currentState);
    }

    public bool isState(string name)
    {
        if (currentState.Name == name) return true;
        else return false;
    }
}

public interface IState
{
    string Name { get; }

    public void OnEnter(StateController controller);
    public void UpdateState(StateController controller);
    public void OnPause(StateController controller);
    public void OnExit(StateController controller);
}