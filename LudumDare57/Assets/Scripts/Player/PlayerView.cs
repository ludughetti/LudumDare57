using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _controller;
    [SerializeField] private PlayerController _player;
    [SerializeField] private string _xParameter = "X";
    [SerializeField] private string _yParameter = "Y";

    private void Update()
    {
        _controller.SetFloat(_xParameter,_player.CurrentDirection.x);
        _controller.SetFloat(_yParameter, _player.CurrentDirection.y);
    }
}
