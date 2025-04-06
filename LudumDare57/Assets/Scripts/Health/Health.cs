using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IAttackable
{
    [SerializeField] private int _maxLife = 10;

    [Header("Audio")]
    [SerializeField] private AudioDataSource audioManager;
    [SerializeField] private AudioConfig takeDamageAudio;

    private int _currentHealth;

    public Action<int> MaxLifeEvent;
    public Action<int> OnHealthChange;
    public Action IsDeadEvent;

    public bool IsAlive { get; set; }

    private void Awake()
    {
        _currentHealth = _maxLife;
        IsAlive = true;

        MaxLifeEvent?.Invoke(_maxLife);
    }

    public void TakeDamage()
    {
        if (!IsAlive)
            return;

        _currentHealth--;

        if (audioManager.DataInstance != null)
            audioManager.DataInstance.PlayAudio(takeDamageAudio);

        if (_currentHealth <= 0)
        {
            IsAlive = false;
            IsDeadEvent?.Invoke();
        }

        OnHealthChange?.Invoke(_currentHealth);
    }

    public void ResetHP()
    {
        _currentHealth = _maxLife;
        IsAlive = true;
        MaxLifeEvent?.Invoke(_maxLife);
    }
}
