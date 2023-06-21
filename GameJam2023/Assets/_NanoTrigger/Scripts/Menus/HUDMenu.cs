using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class HUDMenu : Menu
{
    public TMP_Text statusText;
    public TMP_Text playtimeText;

    private void Update()
    {
        playtimeText.text = GameManager.FormatSeconds(GameManager.Instance.playtime);
        statusText.text = "Health: " + Math.Round(GameManager.Instance.ship.GetComponent<PlayerController>().LifePercentage, 1) + "%";
    }
}
