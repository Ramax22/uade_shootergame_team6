using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _aboutScreen;

    private void Awake()
    {
        _mainMenu.SetActive(true);
        _aboutScreen.SetActive(false);
    }

    // Funcion de botones del Main Menu

    //acceder a la pantalla de About
    public void ChangeToAboutScreen()
    {
        _mainMenu.SetActive(false);
        _aboutScreen.SetActive(true);
    }

    // Funcion de botones de About

    //retroceder al main menu
    public void ReturnToMainMenu()
    {
        _mainMenu.SetActive(true);
        _aboutScreen.SetActive(false);
    }
}