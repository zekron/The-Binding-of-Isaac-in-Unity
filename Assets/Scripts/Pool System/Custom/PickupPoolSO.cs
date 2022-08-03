using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pickup Pool SO",menuName = "Scriptable Object/Object Prefab Pool/Pickup Pool")]
public class PickupPoolSO : ObjectPoolPrefabSO
{
    public GameObject PrefabBomb => Pools[0].Prefab;
    public GameObject PrefabCard => Pools[1].Prefab;
    public GameObject PrefabChest => Pools[2].Prefab;
    public GameObject PrefabCoin => Pools[3].Prefab;
    public GameObject PrefabHeart => Pools[4].Prefab;
    public GameObject PrefabKey => Pools[5].Prefab;
    public GameObject PrefabPill => Pools[6].Prefab;
}
