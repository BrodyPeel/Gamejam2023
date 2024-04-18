using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : Menu
{
    public GameObject backButton;

    public void KeyMapping()
    {
        //Open Key Mapping Menu
        AudioController.Instance.PlaySFX(SFX.Select);
        MenuController.Instance.ShowMenu(MenuController.Instance.keyMappingMenu);
    }

    public void Back()
    { 
        AudioController.Instance.PlaySFX(SFX.Select);
        MenuController.Instance.ShowMenu(MenuController.Instance.titleMenu);
    }
}
