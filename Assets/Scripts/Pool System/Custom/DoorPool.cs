using UnityEngine;

[System.Serializable]
public class DoorPool : ObjectPool
{
    [SerializeField] private DoorObjectType objectType;

    public DoorObjectType ObjectType => objectType;

    public DoorPool(GameObject prefab, int size = 1) : base(prefab, size)
    {
    }
}
