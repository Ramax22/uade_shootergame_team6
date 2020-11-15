using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD_Manager : MonoBehaviour
{
    //Vars
    [Header("Main screens")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _inGameHUD;
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _loseScreen;

    [Header("HUD Screen vars")]
    [SerializeField] Text _healthText;
    [SerializeField] Text _timeText;
    [SerializeField] Text _waveText;

    [Header("Win Screen vars")]
    [SerializeField] Text _scoreText;

    [Header("Lose Screen vars")]
    [SerializeField] Text _scoreTextLose;

    [Header("Extras")]
    [SerializeField] EntityModel _playerModel;
    [SerializeField] WaveManager _waveManager;

    float _actualTime;
    bool _pauseActive;

    private void Awake()
    {
        _actualTime = 0f;
        _pauseActive = false;
    }

    private void Update()
    {
        UpdateRoundTime();
        UpdateLife();
        UpdateWave();
        PauseMenuManager();
    }

    //Funcion para ir al Main Menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    #region ~~~ HUD MENU ~~~
    //Funcion para actualizar la vida del jugador durante la partida
    void UpdateLife()
    {
        var life = _playerModel.GetHealth();
        _healthText.text = "HP: " + life;
    }

    //Funcion para actualizar el tiempo de partida
    void UpdateRoundTime()
    {
        _actualTime += Time.deltaTime;
        int aux = (int)_actualTime;
        _timeText.text = "CURRENT TIME: " + aux + "s";
    }

    //funcion para actualizar el numero de la wave
    void UpdateWave()
    {
        int wave = _waveManager.CurrentWave + 1;
        _waveText.text = "WAVE: " + wave;
    }
    #endregion

    #region ~~~ PAUSE MENU ~~~
    //Funcion que maneja el menu de pausa
    void PauseMenuManager()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            _pauseActive = !_pauseActive;

            if (_pauseActive)
                EnterInGamePauseMenu();
            else
                GetOutInGamePauseMenu();
        }
    }

    //Funcion para abrir el menu de pausa
    void EnterInGamePauseMenu()
    {
        _pauseMenu.SetActive(true);
        _inGameHUD.SetActive(false);
        Time.timeScale = 0;
    }

    public void GetOutInGamePauseMenu()
    {
        _pauseMenu.SetActive(false);
        _inGameHUD.SetActive(true);
        Time.timeScale = 1;
    }
    #endregion

    #region ~~~ WIN MENU ~~~
    //Funcion para actualizar el valor del texto de score
    public void WinGame()
    {
        _winScreen.SetActive(true);
        Time.timeScale = 0;
        _scoreText.text = "Your playtime was: " + _actualTime + "s";
        _pauseMenu.SetActive(false);
        _inGameHUD.SetActive(false);
    }
    #endregion

    #region ~~~ LOSE MENU ~~~
    public void LoseGame()
    {
        _loseScreen.SetActive(true);
        Time.timeScale = 0;
        _scoreTextLose.text = "Your playtime was: " + _actualTime + "s";
        _pauseMenu.SetActive(false);
        _inGameHUD.SetActive(false);
    }
    #endregion
}