using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsMenu : Menu
{
    public TMP_Text resultsText;
    public TMP_Text playtimeText;
    public TMP_Text scoreText;

    public GameObject nextButton;

    public void NextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
