using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _controller;
    [SerializeField] private PlayerController _player;
    [SerializeField] private string _xParameter = "X";
    [SerializeField] private string _yParameter = "Y";
    [SerializeField] private string _isMovingParameter = "IsMoving";

    private void OnEnable()
    {
        _player.IsMovingEvent += HandleIsMoving;
    }

    private void OnDisable()
    {
        _player.IsMovingEvent -= HandleIsMoving;
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
}
