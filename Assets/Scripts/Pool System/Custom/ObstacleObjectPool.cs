using UnityEngine;

[System.Serializable]
public class ObstacleObjectPool : ObjectPool
{
    [SerializeField] private ObstacleObjectType objectType;

    public ObstacleObjectType ObjectType => objectType;

    public ObstacleObjectPool(GameObject prefab, int size = 1) : base(prefab, size)
    {
    }
}