using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameplayConfigs _gameplayConfigs;
    [SerializeField] private InputHandler _inputHandler;

    [SerializeField] private bool _enableLog = true;

    private Vector2 _currentDirection = Vector2.zero;
    private float _currentSpeed = 0f;
    private bool _isMoving = false;

    private Coroutine _movementCoroutine;

    public Vector2 CurrentDirection {  get { return _currentDirection; } }

    private void OnEnable()
    {
        _inputHandler.MovementEvent += HandleMovement;
        _inputHandler.StopMovementEvent += StopMovement;
    }

    private void OnDisable()
    {
        _inputHandler.MovementEvent -= HandleMovement;
        _inputHandler.StopMovementEvent -= StopMovement;
    }

    private void Update()
    {
        transform.position += (Vector3)(_currentDirection * _currentSpeed * Time.deltaTime);
    }

    private void HandleMovement(Vector2 newDirection)
    {
        _isMoving = true;
        StartNewMovementCoroutine(SmoothMovement(newDirection));
    }

    private void StopMovement()
    {
        _isMoving = false;
        StartNewMovementCoroutine(Decelerate());
    }

    private IEnumerator SmoothMovement(Vector2 newDirection)
    {
        _isMoving = true;

        while (_isMoving)
        {
            yield return null;

            float dot = Vector2.Dot(_currentDirection, newDirection.normalized);

            if (dot < -0.1f)
            {
                _currentSpeed = 0;
            }
            else if (_currentSpeed < _gameplayConfigs.PlayerBaseMovSpeed)
            {
                _currentSpeed += Time.deltaTime * _gameplayConfigs.PlayerAcceleration;
                _currentSpeed = Mathf.Min(_currentSpeed, _gameplayConfigs.PlayerBaseMovSpeed);
            }

            _currentDirection = newDirection;

            if (_currentDirection.y > 0.1f)
            {
                _currentDirection.y -= _currentDirection.y / 2;
            }

            if (_enableLog)
                Debug.Log($"Speed: {_currentSpeed}, Dir: {_currentDirection}, Dot: {dot}");
        }
    }

    private IEnumerator Decelerate()
    {
        _isMoving = false;

        while (_currentSpeed > 0)
        {
            yield return null;

            _currentSpeed -= Time.deltaTime * _gameplayConfigs.SinkSpeed;
            _currentSpeed = Mathf.Max(0, _currentSpeed);

            _currentDirection = Vector2.Lerp(_currentDirection, Vector2.down, Time.deltaTime * _gameplayConfigs.SinkSpeed);

            if (_currentDirection.y > 0.1f)
            {
                _currentDirection.y -= _currentDirection.y / 2;
            }
        }

        if (!_isMoving)
        {
            _movementCoroutine = StartCoroutine(SinkLogic(_currentSpeed));
        }
    }

    private IEnumerator SinkLogic(float initialSpeed)
    {
        _currentDirection = Vector2.down;
        _currentSpeed = initialSpeed;

        while (!_isMoving)
        {
            yield return null;

            _currentSpeed += Time.deltaTime * _gameplayConfigs.SinkSpeed;
            _currentSpeed = Mathf.Min(_currentSpeed, _gameplayConfigs.PlayerBaseMovSpeed / 2f);

            if (_enableLog)
                Debug.Log($"Sinking... Speed: {_currentSpeed}");
        }
    }

    private void StartNewMovementCoroutine(IEnumerator routine)
    {
        if (_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);

        _movementCoroutine = StartCoroutine(routine);
    }
}