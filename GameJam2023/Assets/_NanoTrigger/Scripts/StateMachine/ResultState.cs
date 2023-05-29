using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultState : IState
{
    public string Name => "ResultsState";

    public void OnEnter(StateController sc)
    {
        MenuController.Instance.screenFader.FadeToBlack();
        TMPro.TMP_Text text = MenuController.Instance.resultsMenu.resultsText;

        text.text = (GameManager.Instance.procedureSuccess) ? "Procedure Complete" : "Procedure Failed";

        MenuController.Instance.resultsMenu.playtimeText.text = "Duration: " + GameManager.FormatSeconds(GameManager.Instance.playtime);
        //MenuController.Instance.resultsMenu.scoreText.text = "Score: " + GameManager.Instance.score;

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
