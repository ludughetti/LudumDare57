using System;
using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private float _flashDuration = 0.2f;

    [SerializeField] private Animator _controller;
    [SerializeField] private PlayerController _player;
    [SerializeField] private string _xParameter = "X";
    [SerializeField] private string _yParameter = "Y";
    [SerializeField] private string _isMovingParameter = "IsMoving";
    [SerializeField] private string _isDeadParameter = "IsDead";

    private bool _isMakingDamageFeedback = false;

    private void OnEnable()
    {
        _player.AliveEvent += HandleIsAlive;
        _player.IsMovingEvent += HandleIsMoving;
        _player.DeadEvent += HandleIsDead;
        _player.IsDamagedEvent += HandleIsAttacked;
    }

    private void OnDisable()
    {
        _player.AliveEvent -= HandleIsAlive;
        _player.IsMovingEvent -= HandleIsMoving;
        _player.DeadEvent -= HandleIsDead;
        _player.IsDamagedEvent -= HandleIsAttacked;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _controller.SetFloat(_xParameter,_player.CurrentDirection.x);
        _controller.SetFloat(_yParameter, _player.CurrentDirection.y);
    }

    private void HandleIsMoving(bool isMoving)
    {
        _controller.SetBool(_isMovingParameter,isMoving);
    }

    private void HandleIsDead()
    {
        _controller.SetBool(_isDeadParameter, true);
    }

    private void HandleIsAlive()
    {
        _controller.SetBool(_isDeadParameter, false);
    }

    private void HandleIsAttacked()
    {
        if (_isMakingDamageFeedback)
            return;

        _isMakingDamageFeedback = true;
        StartCoroutine(FlashDamage());
    }

    private IEnumerator FlashDamage()
    {
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = _damageColor;

        for (int i = 0; i < 3; i++)
        {
            _spriteRenderer.color = _damageColor;
            yield return new WaitForSeconds(_flashDuration);
            _spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(_flashDuration);
        }

        _isMakingDamageFeedback = false;
    }
}
