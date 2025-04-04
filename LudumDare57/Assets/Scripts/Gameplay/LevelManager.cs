using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerController _player;

    public Action<bool> triggerEndgame;

    private void OnEnable()
    {
        _gameManager.StartGame += HandleStartGame;
        _gameManager.EndGame += HandlePlayerDead;
    }

    private void OnDisable()
    {
        _gameManager.StartGame -= HandleStartGame;
        _gameManager.EndGame -= HandlePlayerDead;
    }

    private void HandleStartGame()
    {
        _player.HandleStartGame();
    }

    private void HandlePlayerDead()
    {
        Debug.Log("Player lost!");
        triggerEndgame?.Invoke(false);
    }
}
