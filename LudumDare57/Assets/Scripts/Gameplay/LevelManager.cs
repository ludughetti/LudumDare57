using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    public Action<bool> triggerEndgame;

    private void OnEnable()
    {
        if(_player != null)
            _player.GameLost += HandlePlayerDead;
    }

    private void OnDisable()
    {
        if (_player != null)
            _player.GameLost -= HandlePlayerDead;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
            HandlePlayerWin();
    }

    private void HandlePlayerDead()
    {
        Debug.Log("Player lost!");
        triggerEndgame?.Invoke(false);
    }

    private void HandlePlayerWin()
    {
        Debug.Log("Player Win!");
        triggerEndgame?.Invoke(true);
    }
}
