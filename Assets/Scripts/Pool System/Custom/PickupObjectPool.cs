using UnityEngine;

[System.Serializable]
public class PickupObjectPool : ObjectPool
{
    [SerializeField] private PickupObjectType objectType;

    public PickupObjectType ObjectType => objectType;

    public PickupObjectPool(GameObject prefab, int size = 1) : base(prefab, size)
    {
    }
}

public enum PickupObjectType
{
    Bomb,
    Card,
    Chest,
    Coin,
    Heart,
    Key,
    Pill,
}
