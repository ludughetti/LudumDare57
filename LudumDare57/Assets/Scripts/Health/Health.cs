using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IAttackable
{
    [SerializeField] private int _maxLife = 10;

    private int _currentHealth;

    public Action<int> MaxLifeEvent;
    public Action<int> OnHealthChange;
    public Action IsDeadEvent;

    public bool IsAlive { get; set; }

    private void Awake()
    {
        _currentHealth = _maxLife;
        IsAlive = true;
    }

    private void Start()
    {
        MaxLifeEvent?.Invoke(_maxLife);
    }

    public void TakeDamage()
    {
        if (!IsAlive)
            return;
        _currentHealth--;

        if (_currentHealth < 0)
        {
            IsAlive = false;
            IsDeadEvent?.Invoke();
        }

        OnHealthChange?.Invoke(_currentHealth);
    }
}
