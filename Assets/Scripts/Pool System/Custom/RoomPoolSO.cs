using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Pool SO", menuName = "Scriptable Object/Object Prefab Pool/Room Pool")]
public class RoomPoolSO : ScriptableObject, IPools
{
    public Dictionary<RoomObjectType, GameObject> roomObjectDic;
    [SerializeField] private RoomPool[] roomPools;

    public ObjectPool[] Pools => roomPools;

    public void Initialize()
    {
        roomObjectDic = new Dictionary<RoomObjectType, GameObject>();

        foreach (var pool in roomPools)
        {
#if UNITY_EDITOR
            if (roomObjectDic.ContainsKey(pool.ObjectType))
            {
                Debug.LogError("Same prefab type in multiple pools! Prefab type: " + pool.ObjectType);

                continue;
            }
#endif
            roomObjectDic.Add(pool.ObjectType, pool.Prefab);
        }
    }
}
