using UnityEngine;

public class GameplayConfigs : MonoBehaviour
{
    [SerializeField] private float PlayerBaseMovSpeed;
    [SerializeField] private bool PlayerSkillAvailableUponStart;

    public float GetPlayerBaseMovSpeed()
    {
        return PlayerBaseMovSpeed;
    }

    public bool IsPlayerSkillAvailableUponStart()
    {
        return PlayerSkillAvailableUponStart;
    }
}
