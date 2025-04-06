using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private NavigationManager _navManager;
    [SerializeField] private GameplayConfigs _gameplayConfigs;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private OxigenLogic _oxigenLogic;
    [SerializeField] private Health _health;
    [Header("Parameters")]
    [SerializeField] private float _timeToDead = 0.5f;
    [SerializeField] private bool _enableLog = true;
    [SerializeField] private Transform _spawnPoint;

    private Vector2 _currentDirection = Vector2.zero;
    private Vector2 _currentInput = Vector2.zero;

    private float _currentSpeed = 0f;
    private bool _isMoving = false;
    private bool _canMove = true;
    private bool _isImpulsed = false;
    private bool _isDying = false;

    private bool _gameStarted = false;

    private Coroutine _movementCoroutine;

    public Vector2 CurrentDirection {  get { return _currentDirection; } }

    public Action IsDamagedEvent;
    public Action<bool> IsMovingEvent;
    public Action AliveEvent;
    public Action DeadEvent;
    public Action GameLost;

    private void OnEnable()
    {
        _inputHandler.MovementEvent += HandleMovement;
        _inputHandler.StopMovementEvent += StopMovement;
        _oxigenLogic.AllOxigenLostEvent += HandleDead;
        _health.IsDeadEvent += HandleDead;
        _health.OnHealthChange += (int temp) => IsDamagedEvent?.Invoke();
        _navManager.triggerGameStart += ResetPlayer;
    }

    private void OnDisable()
    {
        _inputHandler.MovementEvent -= HandleMovement;
        _inputHandler.StopMovementEvent -= StopMovement;
        _oxigenLogic.AllOxigenLostEvent -= HandleDead;
        _health.IsDeadEvent -= HandleDead;
        _health.OnHealthChange -= (int temp) => IsDamagedEvent?.Invoke();
        _navManager.triggerGameStart -= ResetPlayer;
    }

    private void Update()
    {
        if (!_gameStarted)
            return;

        transform.position += (Vector3)(_currentDirection * _currentSpeed * Time.deltaTime);
    }

    [ContextMenu("Add Up Impulse")]
    private void AddUpImpulseContextMenu()
    {
        AddUpImpulse(2f);
    }

    public void AddUpImpulse(float movementToAdd)
    {
        _isImpulsed = true;
        _currentDirection.y += movementToAdd;
    }

    public void StopImpulse()
    {
        _isImpulsed = false;
    }

    private void HandleMovement(Vector2 newDirection)
    {
        if (!_canMove || !_gameStarted)
            return;

        _currentInput = newDirection;

        if (newDirection.y > 0.1f)
        {
            _currentInput.y = newDirection.y * _gameplayConfigs.VerticalUpModifier;
        }

        IsMovingEvent?.Invoke(true);
        _isMoving = true;
        StartNewMovementCoroutine(SmoothMovement(_currentInput));
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

            _currentDirection.x = newDirection.x;

            if (!_isImpulsed)
            {
                _currentDirection.y = newDirection.y;
                _currentDirection.y = Mathf.Lerp(_currentDirection.y, -1f, Time.deltaTime * _gameplayConfigs.SinkSpeed);
            }
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
            IsMovingEvent?.Invoke(false);
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

    private void HandleDead()
    {
        if (_isDying)
            return;

        _isDying = true;
        _canMove = false;
        DeadEvent?.Invoke();
        StopAllCoroutines();
        StopMovement();
        StartCoroutine(DeadLogic());
    }

    private IEnumerator DeadLogic()
    {
        yield return new WaitForSeconds(_timeToDead);
        GameLost?.Invoke();
    }

    private void ResetPlayer()
    {
        gameObject.transform.position = _spawnPoint.position;
        _gameStarted = true;
        _health.enabled = true;
        _health.ResetHP();
        AliveEvent?.Invoke();
        _oxigenLogic.enabled = true;
        _oxigenLogic.ResetOxygen();
        _canMove = true;
        _isDying = false;
        _currentDirection = Vector2.zero;
        _currentInput = Vector2.zero;
        _currentSpeed = 0;
    }
}