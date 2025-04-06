using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxigenLogic : MonoBehaviour
{
    [SerializeField] private float _maxOxigen = 100f;
    [SerializeField] private float _oxigenLostPerSecond = 1.0f;

    [Header("Audio")]
    [SerializeField] private AudioDataSource audioManager;
    [SerializeField] private AudioConfig getOxigenAudio;

    private float _currentOxigen;

    public Action<float,float> CurrentAndFullOxigenEvent;
    public Action AllOxigenLostEvent;

    private void Awake()
    {
        _currentOxigen = _maxOxigen;
    }

    private void Update()
    {
        HandleOxigenLost();
    }

    public void AddOxigen(float oxigenToAdd)
    {
        _currentOxigen += oxigenToAdd;
        if(_currentOxigen > _maxOxigen)
        {
            _currentOxigen = _maxOxigen;

            if (audioManager.DataInstance != null)
                audioManager.DataInstance.PlayAudio(getOxigenAudio);
        }

        CurrentAndFullOxigenEvent?.Invoke(_currentOxigen, _maxOxigen);
    }

    private void HandleOxigenLost()
    {
        if (_currentOxigen <= 0f)
            return;

        _currentOxigen -= _oxigenLostPerSecond * Time.deltaTime;
        if (_currentOxigen <= 0f)
        {
            _currentOxigen = 0f;
            AllOxigenLostEvent?.Invoke();
            Debug.Log($"{name}: Die!");
        }

        CurrentAndFullOxigenEvent?.Invoke(_currentOxigen, _maxOxigen);
    }

    public void ResetOxygen()
    {
        _currentOxigen = _maxOxigen;
        CurrentAndFullOxigenEvent?.Invoke(_currentOxigen, _maxOxigen);
    }
}
