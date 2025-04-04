using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameplayConfigs _gameplayConfigs;
    
    [SerializeField] private float _movSpeed { get; set; }
    [SerializeField] private bool _skillAvailable { get; set; }

    public void HandleStartGame()
    {
        _movSpeed = _gameplayConfigs.GetPlayerBaseMovSpeed();
        _skillAvailable = _gameplayConfigs.IsPlayerSkillAvailableUponStart();
    }
}
