using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player", fileName = "Player")]
public class GameplayConfigs : ScriptableObject
{
    [SerializeField] private float _playerBaseMovSpeed = 3.5f;
    [SerializeField] private float _playerAcceleration = 5f;
    [SerializeField] private float _sinkSpeed = 1;
    [SerializeField] private float _waterFriction = 0.5f;
    [SerializeField][Range(0f, 1f)] public float VerticalUpModifier = 0.5f;
    [SerializeField] private bool PlayerSkillAvailableUponStart;

    public float PlayerBaseMovSpeed { get { return _playerBaseMovSpeed; } }
    public float PlayerAcceleration { get { return _playerAcceleration; } }
    public float SinkSpeed { get { return _sinkSpeed; } }
    public float WaterFriction { get { return _waterFriction; } }
    public bool IsPlayerSkillAvailableUponStart()
    {
        return PlayerSkillAvailableUponStart;
    }
}
