using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    [SerializeField] private Menu[] menus;
    [SerializeField] private Menu currentMenu;

    public ResultsMenu resultsMenu;
    public PauseMenu pauseMenu;

    public ScreenFader screenFader;

    public EventSystem eventSystem;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initially show only the Title menu
        ShowMenu(0);
    }

    public void Pause()
    {
        
        if (GameManager.Instance.state.isState("PlayState"))
        {
            GameManager.Instance.state.ChangeState(GameManager.Instance.state.pauseState);
            return;
        }
        else if(GameManager.Instance.state.isState("PauseState"))
        {
            GameManager.Instance.state.ChangeState(GameManager.Instance.state.playState);
            return;
        }
    }

    public void EscapeKey()
    {
        Application.Quit();
    }

    public void ShowMenu(int menuIndex)
    {
        if(currentMenu != null)
        {
            currentMenu.Close();
        }

        menus[menuIndex].Open();
        currentMenu = menus[menuIndex];
    }

    public void ShowMenu(Menu menu)
    {
        if (currentMenu != null)
        {
            currentMenu.Close();
        }

        menu.Open();
        currentMenu = menu;
    }
}
