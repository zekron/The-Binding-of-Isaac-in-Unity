using UnityEngine;

//[CreateAssetMenu(fileName ="New Pool Prefab",menuName = "Scriptable Object/Object Prefab Pool/...")]
public abstract class ObjectPoolPrefabSO : ScriptableObject
{
    public ObjectPool[] Pools;
}
