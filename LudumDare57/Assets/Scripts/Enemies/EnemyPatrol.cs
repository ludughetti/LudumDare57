using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyConfig config;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float chaseDuration = 3f;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D _rigidbody;
    private Transform _playerTarget;
    private int _currentWaypointIndex = 0;
    private bool _isChasing = false;
    private Coroutine _stopChaseCoroutine;

    public event Action<EnemyStates> OnAction;
    public event Action<EnemyConfig> OnViewSetup;

    public Rigidbody2D Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OnViewSetup?.Invoke(config);
        OnAction?.Invoke(EnemyStates.WALK);
    }

    private void FixedUpdate()
    {
        if (_isChasing && _playerTarget != null)
            MoveTowards(_playerTarget.position, chaseSpeed);

        else if (waypoints.Length > 0)
            Patrol();
    }

    private void Patrol()
    {
        Transform targetWaypoint = waypoints[_currentWaypointIndex];
        MoveTowards(targetWaypoint.position, patrolSpeed);

        float distance = Vector2.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        _rigidbody.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            _playerTarget = other.transform;

            _isChasing = true;
            OnAction?.Invoke(EnemyStates.WALK);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            _playerTarget = other.transform;

            if (_stopChaseCoroutine != null)
                StopCoroutine(_stopChaseCoroutine);

            _stopChaseCoroutine = StartCoroutine(StopChaseCoroutine());
        }
    }

    private bool IsPlayer(Collider2D collider)
    {
        return ((1 << collider.gameObject.layer) & playerLayer) != 0;
    }

    private IEnumerator StopChaseCoroutine()
    {
        yield return new WaitForSeconds(chaseDuration);

        _isChasing = false;
        _playerTarget = null;
        _rigidbody.velocity = Vector2.zero;
        OnAction?.Invoke(EnemyStates.WALK);
    }
}
