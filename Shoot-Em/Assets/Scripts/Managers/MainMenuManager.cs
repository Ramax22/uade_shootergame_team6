using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _playButton;
    [SerializeField] GameObject _aboutButton;
    [SerializeField] GameObject _exitButton;
    [SerializeField] GameObject _aboutMenu;

    // Funcion de botones del Main Menu
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    //acceder a la pantalla de About
    public void ChangeToAboutScreen()
    {
        _playButton.SetActive(false);
        _aboutButton.SetActive(false);
        _exitButton.SetActive(false);
        _aboutMenu.SetActive(true);
    }

    // Funcion de botones de About

    //retroceder al main menu
    public void ReturnToMainMenu()
    {
        _aboutMenu.SetActive(false);
        _playButton.SetActive(true);
        _aboutButton.SetActive(true);
        _exitButton.SetActive(true);
    }
}