using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Door Pool SO", menuName = "Scriptable Object/Object Prefab Pool/Door Pool")]
public class DoorObjectPoolSO : ScriptableObject, IPools
{
    public Dictionary<DoorObjectType, GameObject> DoorObjectDic;
    [SerializeField] private DoorPool[] doorPools;

    public ObjectPool[] Pools => doorPools;

    public void Initialize()
    {
        DoorObjectDic = new Dictionary<DoorObjectType, GameObject>();

        foreach (var pool in doorPools)
        {
#if UNITY_EDITOR
            if (DoorObjectDic.ContainsKey(pool.ObjectType))
            {
                Debug.LogError("Same prefab type in multiple pools! Prefab type: " + pool.ObjectType);

                continue;
            }
#endif
            DoorObjectDic.Add(pool.ObjectType, pool.Prefab);
        }
    }
}
