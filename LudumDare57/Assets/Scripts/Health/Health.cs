using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IAttackable
{
    [SerializeField] private int _maxLife = 10;

    private int _currentHealth;
    private bool _isAlive = true;

    public Action<int> MaxLifeEvent;
    public Action<int> OnHealthChange;
    public Action IsDeadEvent;

    private void Awake()
    {
        _currentHealth = _maxLife;
    }

    private void Start()
    {
        MaxLifeEvent?.Invoke(_maxLife);
    }

    public void TakeDamage()
    {
        if (!_isAlive)
            return;
        _currentHealth--;

        if (_currentHealth < 0)
        {
            _isAlive = false;
            IsDeadEvent?.Invoke();
        }

        OnHealthChange?.Invoke(_currentHealth);
    }
}
