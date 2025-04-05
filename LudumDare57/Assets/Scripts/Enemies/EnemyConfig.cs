using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Enemy", fileName = "EnemyCfg_", order = 0)]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField] public string ID { get; private set; }

    [field: SerializeField] public AnimatorOverrideController AnimatorOverrideController { get; private set; }
}
