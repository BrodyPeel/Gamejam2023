using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMenu : Menu
{
    public TMP_Text statusText;

    private void Update()
    {
        statusText.text = "Status: " + GameManager.Instance.ship.GetComponent<PlayerController>().LifePercentage + "%";
    }
}
