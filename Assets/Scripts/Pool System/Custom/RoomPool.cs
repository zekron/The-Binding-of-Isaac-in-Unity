using UnityEngine;

[System.Serializable]
public class RoomPool : ObjectPool
{
    [SerializeField] private RoomObjectType objectType;

    public RoomObjectType ObjectType => objectType;

    public RoomPool(GameObject prefab, int size = 1) : base(prefab, size)
    {
    }
}
