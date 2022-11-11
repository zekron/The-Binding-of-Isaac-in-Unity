using AssetBundleFramework;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectPoolManager : MonoBehaviour
{
    private static PickupPoolSO pickupPools;
    private static ObstaclePoolSO obstaclePools;
    private static RoomPoolSO roomPools;
    private static DoorObjectPoolSO doorPools;

    protected void Awake()
    {
        pickupPools = AssetBundleManager.Instance.LoadAsset<PickupPoolSO>("Pickup Pool SO.asset");
        obstaclePools = AssetBundleManager.Instance.LoadAsset<ObstaclePoolSO>("Obstacle Pool SO.asset");
        doorPools = AssetBundleManager.Instance.LoadAsset<DoorObjectPoolSO>("Door Pool SO.asset");
        roomPools = AssetBundleManager.Instance.LoadAsset<RoomPoolSO>("Room Pool SO.asset");

        pickupPools.Initialize();
        obstaclePools.Initialize();
        doorPools.Initialize();
        roomPools.Initialize();

        ObjectPoolManager.Initialize(pickupPools.Pools);
        ObjectPoolManager.Initialize(obstaclePools.Pools);
        ObjectPoolManager.Initialize(roomPools.Pools);
        ObjectPoolManager.Initialize(doorPools.Pools);
    }

    private static bool TypeInDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey type)
    {
        if (dic.ContainsKey(type)) return true;
        else
        {
            Debug.LogError($"Pool Manager could NOT find prefab type: {type}");
            return false;
        }
    }

    #region PickupObject
    public static GameObject Release(PickupObjectType type)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(pickupPools.PickupObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(pickupPools.PickupObjectDic[type]);
    }
    public static GameObject Release(PickupObjectType type, Vector3 position)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(pickupPools.PickupObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(pickupPools.PickupObjectDic[type], position);
    }
    public static GameObject Release(PickupObjectType type, Vector3 position, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(pickupPools.PickupObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(pickupPools.PickupObjectDic[type], position, Quaternion.identity, localScale);
    }
    public static GameObject Release(PickupObjectType type, Vector3 position, Quaternion rotation, Transform parent)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(pickupPools.PickupObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(pickupPools.PickupObjectDic[type], position, rotation, parent);
    }
    #endregion

    #region ObstacleObject
    public static GameObject Release(ObstacleObjectType type)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(obstaclePools.ObstacleObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(obstaclePools.ObstacleObjectDic[type]);
    }
    public static GameObject Release(ObstacleObjectType type, Vector3 position)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(obstaclePools.ObstacleObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(obstaclePools.ObstacleObjectDic[type], position);
    }
    #endregion

    #region DoorObject
    public static GameObject Release(DoorObjectType type)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(doorPools.DoorObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(doorPools.DoorObjectDic[type]);
    }
    public static GameObject Release(DoorObjectType type, Vector3 position, Quaternion rotation, Transform parent)
    {
#if UNITY_EDITOR
        if (!TypeInDictionary(doorPools.DoorObjectDic, type)) return null;
#endif
        return ObjectPoolManager.Release(doorPools.DoorObjectDic[type], position, rotation, parent);
    }
    #endregion
}
