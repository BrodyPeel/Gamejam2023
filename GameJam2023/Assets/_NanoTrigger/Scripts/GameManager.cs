using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Reference to Ship Object. May move later
    public GameObject ship;
    new public CameraController camera;
    public StateController state;
    public LevelManager levelManager;

    public float playtime;
    public int score;

    public bool procedureSuccess = false;

    public CanvasGroup noControllerCanvasGroup;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(joystickNames[i]))
            {
                noControllerCanvasGroup.alpha = 0f;
            }
            else
            {
                noControllerCanvasGroup.alpha = 1f;
            }
        }
    }

    public static string FormatSeconds(float totalSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
        return string.Format("{0}:{1:D2}", time.Minutes, time.Seconds);
    }
}
