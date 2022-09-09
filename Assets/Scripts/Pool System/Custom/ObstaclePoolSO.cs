using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleObjectType
{
    Rock,

    Poop_Normal,
    Poop_Corny,
    Poop_Gold,
    Poop_Red,

    FIrePlace,

    Pedestal,
}

[CreateAssetMenu(fileName = "Obstacle Pool SO", menuName = "Scriptable Object/Object Prefab Pool/Obstacle Pool")]
public class ObstaclePoolSO : ScriptableObject, IPools
{
    public Dictionary<ObstacleObjectType, GameObject> ObstacleObjectDic;
    [SerializeField] private ObstacleObjectPool[] obstaclePools;

    public ObjectPool[] Pools => obstaclePools;

    public void Initialize()
    {
        ObstacleObjectDic = new Dictionary<ObstacleObjectType, GameObject>();

        foreach (var pool in obstaclePools)
        {
#if UNITY_EDITOR
            if (ObstacleObjectDic.ContainsKey(pool.ObjectType))
            {
                Debug.LogError("Same prefab type in multiple pools! Prefab type: " + pool.ObjectType);

                continue;
            }
#endif
            ObstacleObjectDic.Add(pool.ObjectType, pool.Prefab);
        }
    }
}
