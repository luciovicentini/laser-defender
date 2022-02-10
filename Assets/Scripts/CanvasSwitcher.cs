using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject howToMenu;

    public void showMainMenu()
    {
        mainMenu.SetActive(true);
        howToMenu.SetActive(false);
    }

    public void showHowToMenu()
    {
        mainMenu.SetActive(false);
        howToMenu.SetActive(true);
    }
}
