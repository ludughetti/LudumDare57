using Audio;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemySlide))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyConfig config;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float chaseDuration = 3f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Combat Settings")]
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioDataSource audioManager;
    [SerializeField] private AudioConfig attackAudio;

    private Rigidbody2D _rigidbody;
    private EnemySlide _slide;

    private Transform _playerTarget;
    private int _currentWaypointIndex = 0;

    private bool _isChasing = false;
    private bool _isWaitingToPatrol = false;
    private bool _isAttacking = false;
    private bool _gameStarted = false;

    private bool _playerIsDead = false;

    private Coroutine _stopChaseCoroutine;

    public event Action<EnemyStates> OnAction;
    public event Action<EnemyConfig> OnViewSetup;
    public event Action<Vector2> OnFaceDirection;


    public Rigidbody2D Rigidbody => _rigidbody;

    private void OnEnable()
    {
        if(gameManager)
            gameManager.StartGame += HandleGameStarted;
    }

    private void OnDisable()
    {
        if (gameManager)
            gameManager.StartGame -= HandleGameStarted;

        if (_stopChaseCoroutine != null)
            StopCoroutine(_stopChaseCoroutine);
    }

    private void Awake()
    {
        if (!gameManager)
            Debug.LogError($"{name} GAME MANAGER IS NULL");
        _rigidbody = GetComponent<Rigidbody2D>();
        _slide = GetComponent<EnemySlide>();

        _slide.Setup(_rigidbody);
    }

    private void Start()
    {
        OnViewSetup?.Invoke(config);
        OnAction?.Invoke(EnemyStates.WALK);
    }

    private void FixedUpdate()
    {
        if (_isAttacking || !_gameStarted) return;

        if (_isChasing && _playerTarget != null && !_playerIsDead)
        {
            float distance = Vector2.Distance(transform.position, _playerTarget.position);

            if (distance <= attackDistance) TryAttackTarget();
            else MoveTowards(_playerTarget.position, chaseSpeed);
        }

        else if (!_isWaitingToPatrol && waypoints.Length > 0)
            Patrol();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_gameStarted || _playerIsDead) return;

        if (IsPlayer(other))
        {
            if (_stopChaseCoroutine != null)
                StopCoroutine(_stopChaseCoroutine);

            _playerTarget = other.transform;

            _isChasing = true;
            _isWaitingToPatrol = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!enabled || !_gameStarted) return;
        if (IsPlayer(other))
            _stopChaseCoroutine = StartCoroutine(StopChaseCoroutine());
    }

    private void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        _isWaitingToPatrol = true;

        Transform targetWaypoint = waypoints[_currentWaypointIndex];
        MoveTowards(targetWaypoint.position, patrolSpeed);

        while (Vector2.Distance(transform.position, targetWaypoint.position) > 0.1f)
            yield return null;

        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;

        _slide.OnSlideComplete += HandlePatrolSlideComplete;
        _slide.Slide(_rigidbody.velocity);
        OnAction?.Invoke(EnemyStates.IDLE);
    }

    private void MoveTowards(Vector2 target, float speed, bool isChasing = false)
    {
        OnFaceDirection?.Invoke(target);

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        _rigidbody.velocity = direction * speed;
    }

    private void TryAttackTarget()
    {
        _isAttacking = true;

        OnFaceDirection?.Invoke(_playerTarget.position);
        OnAction?.Invoke(EnemyStates.IDLE);

        _slide.OnSlideComplete += HandleAttackSlideComplete;
        _slide.Slide(_rigidbody.velocity);

        float distance = Vector2.Distance(transform.position, _playerTarget.position);

        if (_playerTarget != null && distance <= attackDistance)
        {
            if (_playerTarget.TryGetComponent<IAttackable>(out var attackable))
            {
                attackable.TakeDamage();

                if (!attackable.IsAlive)
                    _playerIsDead = true;
            }

            if(audioManager.DataInstance != null)
            {
                audioManager.DataInstance.PlayAudio(attackAudio);
                Debug.Log("attack!");
            }

            OnAction?.Invoke(EnemyStates.ATTACK);
        }
    }

    private IEnumerator StopChaseCoroutine()
    {
        yield return new WaitForSeconds(chaseDuration);

        _isChasing = false;

        _playerTarget = null;

        _slide.OnSlideComplete += HandleReturnFromChase;
        _slide.Slide(_rigidbody.velocity);
    }

    private void HandlePatrolSlideComplete()
    {
        _slide.OnSlideComplete -= HandlePatrolSlideComplete;
        _isWaitingToPatrol = false;

        OnAction?.Invoke(EnemyStates.WALK);
    }

    private void HandleReturnFromChase()
    {
        _slide.OnSlideComplete -= HandleReturnFromChase;

        _isWaitingToPatrol = false;
        OnAction?.Invoke(EnemyStates.WALK);

        Patrol();
    }

    private void HandleAttackSlideComplete()
    {
        _slide.OnSlideComplete -= HandleAttackSlideComplete;

        _isAttacking = false;
        _isWaitingToPatrol = false;

        OnAction?.Invoke(EnemyStates.WALK);
    }

    private bool IsPlayer(Collider2D collider)
    {
        return ((1 << collider.gameObject.layer) & playerLayer) != 0;
    }

    private void HandleGameStarted()
    {
        _gameStarted = true;
        _playerIsDead = false;
        _isChasing = false;
        _isWaitingToPatrol = false;
        _isAttacking = false;
    }
}
