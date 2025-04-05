using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyAnimatorView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyPatrol enemy;

    [Header("Animator Parameters")]
    [SerializeField] private string horSpeedParameter = "hor_speed";
    [SerializeField] private string attackTriggerParameter = "attack";
    [SerializeField] private string deathTriggerParameter = "death";
    [SerializeField] private string hurtTriggerParameter = "hurt";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        enemy.OnViewSetup += HandleAnimationConfig;
        enemy.OnAction += HandleActionChange;
        enemy.OnFaceDirection += HandleFacing;

    }

    private void OnDisable()
    {
        enemy.OnViewSetup -= HandleAnimationConfig;
        enemy.OnAction -= HandleActionChange;
        enemy.OnFaceDirection -= HandleFacing;

    }

    private void HandleAnimationConfig(EnemyConfig config)
    {
        if (config.AnimatorOverrideController != null)
            _animator.runtimeAnimatorController = config.AnimatorOverrideController;

        else
            Debug.LogWarning("EnemyConfig has no AnimatorOverrideController assigned.");
    }

    private void HandleFacing(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        if (direction.x != 0)
            _spriteRenderer.flipX = direction.x < 0f;
    }
    private void HandleActionChange(EnemyStates enemyState)
    {      
        switch (enemyState)
        {
            case EnemyStates.IDLE:
                _animator.SetFloat(horSpeedParameter, 0f);
                break;
            case EnemyStates.WALK:
                _animator.SetFloat(horSpeedParameter, 1f);
                break;
            case EnemyStates.DEATH:
                _animator.SetTrigger(deathTriggerParameter);
                break;
            case EnemyStates.HURT:
                _animator.SetTrigger(hurtTriggerParameter);
                break;
            case EnemyStates.ATTACK:
                _animator.SetTrigger(attackTriggerParameter);
                break;

        }
    }
}
