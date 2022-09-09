using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup Pool SO", menuName = "Scriptable Object/Object Prefab Pool/Pickup Pool")]
public class PickupPoolSO : ScriptableObject, IPools
{
    public Dictionary<PickupObjectType, GameObject> PickupObjectDic;
    [SerializeField] private PickupObjectPool[] pickupPools;

    public ObjectPool[] Pools => pickupPools;

    public void Initialize()
    {
        PickupObjectDic = new Dictionary<PickupObjectType, GameObject>();

        foreach (var pool in pickupPools)
        {
#if UNITY_EDITOR
            if (PickupObjectDic.ContainsKey(pool.ObjectType))
            {
                Debug.LogError("Same prefab type in multiple pools! Prefab type: " + pool.ObjectType);

                continue;
            }
#endif
            PickupObjectDic.Add(pool.ObjectType, pool.Prefab);
        }
    }
}
