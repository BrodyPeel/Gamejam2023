using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyMappingMenu : Menu
{
    public GameObject backButton;

    public void Back()
    {
        AudioController.Instance.PlaySFX(SFX.Select);
        MenuController.Instance.ShowMenu(MenuController.Instance.keyMappingMenu);
    }
}
