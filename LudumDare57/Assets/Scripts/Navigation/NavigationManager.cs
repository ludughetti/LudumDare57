using Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    [Tooltip("First menu in the list is the default one")]
    [SerializeField] private List<MenuDataSource> _menusWithId;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private LevelManager _levelManager;

    [SerializeField] private AudioDataSource audioDataSource;
    [SerializeField] private AudioConfig buttonClickAudio;

    [SerializeField] private MenuDataSource _endScreenId;

    public Action triggerGameStart;

    private int _currentMenuIndex = 0;
    private bool _gameStarted = false;
    private bool _isGamePaused = false;

    private void OnEnable()
    {
        inputHandler.PauseEvent += HandleOpenPauseMenu;
        _gameManager.StartGame += HandleStartGame;
        _levelManager.triggerEndgame += HandleEndgame;
    }

    private void OnDisable()
    {
        inputHandler.PauseEvent -= HandleOpenPauseMenu;
        _gameManager.StartGame -= HandleStartGame;
        _levelManager.triggerEndgame -= HandleEndgame;
    }

    private void Start()
    {
        foreach (var menu in _menusWithId)
        {
            if (menu.DataInstance != null)
            {
                menu.DataInstance.Setup();
                menu.DataInstance.OnChangeMenu += HandleChangeMenu;
                menu.DataInstance.gameObject.SetActive(false);
            }
        }

        if (_menusWithId.Count > 0)
        {
            _menusWithId[_currentMenuIndex].DataInstance.gameObject.SetActive(true);
        }
    }

    private void HandleChangeMenu(string id)
    {
        if(!_gameStarted) _gameManager.HandleMenuChange(id);

        if (audioDataSource.DataInstance != null)
            audioDataSource.DataInstance.PlayAudio(buttonClickAudio);

        if (_gameStarted && _menusWithId.Count > 0 && _menusWithId[0].menuId == id)
        {
            var ingameMenu = _menusWithId.FirstOrDefault(menu => menu._isIngameMenu);
            if (ingameMenu != null && ingameMenu.DataInstance != null)
            {
                HandleOpenPauseMenu();
                return;
            }
        }

        for (var i = 0; i < _menusWithId.Count; i++)
        {
            var menuWithId = _menusWithId[i];

            if (menuWithId.menuId == id)
            {
                _menusWithId[_currentMenuIndex].DataInstance.gameObject.SetActive(false);
                menuWithId.DataInstance.gameObject.SetActive(true);
                _currentMenuIndex = i;
                break;
            }
        }
    }

    private void HandleEndgame(bool isWin)
    {
        HandleChangeMenu(_endScreenId.name);
    }

    private void HandleOpenPauseMenu()
    {
        if(!_gameStarted) return;

        _isGamePaused = !_isGamePaused;

        if (_isGamePaused)
            HandleChangeMenu("Settings");

        else
        {
            _menusWithId[_currentMenuIndex].DataInstance.gameObject.SetActive(false);
            HandleChangeMenu("Play");
        }

        _gameManager.HandlePauseGame(_isGamePaused);
    }

    private void HandleStartGame(bool startGame)
    {
        Debug.Log("Game started: " + startGame);
        _gameStarted = startGame;

        if (_gameStarted)
            triggerGameStart?.Invoke();
    }
}
