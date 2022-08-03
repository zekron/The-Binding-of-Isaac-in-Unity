using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Pool SO", menuName = "Scriptable Object/Object Prefab Pool/Obstacle Pool")]
public class ObstaclePoolSO : ObjectPoolPrefabSO
{
    public GameObject RockPrefab => Pools[0].Prefab;
    public GameObject PrefabPoopNormal => Pools[1].Prefab;
    public GameObject PrefabPoopCorny => Pools[2].Prefab;
    public GameObject PrefabPoopGold => Pools[3].Prefab;
    public GameObject PrefabPoopRed => Pools[4].Prefab;
    public GameObject PrefabFirePlace => Pools[5].Prefab;
}
