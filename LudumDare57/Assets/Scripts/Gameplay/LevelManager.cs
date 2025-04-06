using Audio;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    [Header("Audio")]
    [SerializeField] private AudioDataSource audioManager;
    [SerializeField] private AudioConfig loseAudio;
    [SerializeField] private AudioConfig winAudio;

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

        if (audioManager.DataInstance != null)
            audioManager.DataInstance.PlayAudio(loseAudio);
    }

    private void HandlePlayerWin()
    {
        Debug.Log("Player Win!");
        triggerEndgame?.Invoke(true);

        if (audioManager.DataInstance != null)
            audioManager.DataInstance.PlayAudio(winAudio);
    }
}
