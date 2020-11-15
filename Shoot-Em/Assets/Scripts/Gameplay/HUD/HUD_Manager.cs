using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD_Manager : MonoBehaviour
{
    //Vars
    [SerializeField] Text _healthText;
    [SerializeField] Text _timeText;
    [SerializeField] EntityModel _playerModel;
    [SerializeField] Text _waveText;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _inGameHUD;
    [SerializeField] WaveManager _waveManager;

    float _actualTime;
    int _waveNumber;
    bool _pauseActive;

    private void Awake()
    {
        _actualTime = 0f;
        _waveNumber = 0;
        _pauseActive = false;
    }

    private void Update()
    {
        UpdateRoundTime();
        UpdateLife();
        UpdateWave();
        PauseMenuManager();
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Log(_waveManager.CurrentWave);
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

    //Funcion para ir al Main Menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}