using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ChestSO", menuName = "Scriptable Object/Pickups/Chest")]
public class ChestSO : ScriptableObject
{
    public Sprite[] ChestSprites;
    public CustomFrameAnimationClip[] OpenChestClips;
    public PickupPoolSO PickupPool;
    public enum ChestType
    {
        Normal,
        Locked,
        Red,
    }
    public void SpawnReward(ChestType type, Vector3 position)
    {
        var rate = UnityEngine.Random.Range(0f, 1f);
        switch (type)
        {
            case ChestType.Normal:
                if (rate < 0.68f)
                {
                    //spawn 1 ~ 3 pickups
                    var rnd = UnityEngine.Random.value;
                    var cnt = 0;
                    if (rnd < 0.5f) cnt = 1;
                    else if (rnd < 0.8f) cnt = 2;
                    else cnt = 3;

                    SpawnPickups(cnt, position);
                }
                else if (rate < 0.78f)
                {
                    //spawn trinket
                    CustomDebugger.Log("spawn trinket");
                }
                else if (rate < 0.98f)
                {
                    //spawn pill
                    ObjectPoolManager.Release(PickupPool.PrefabPill, position);
                }
                else
                {
                    //spawn smaller chest
                    var chest = ObjectPoolManager.Release(PickupPool.PrefabChest, position, Quaternion.identity, new Vector3(0.7f, 0.7f, 0.7f)).GetComponent<Chest>();
                    chest.SetType(ChestType.Normal);
                }
                break;
            case ChestType.Locked:
                if (rate < 0.2f)
                {
                    //spawn golden chest item
                    CustomDebugger.Log("spawn golden chest item");
                }
                else if (rate < 0.78f)
                {
                    //spawn 2 ~ 6 pickups
                    var rnd = UnityEngine.Random.value;
                    var cnt = 0;
                    if (rnd < 0.3f) cnt = 2;
                    else if (rnd < 0.5f) cnt = 3;
                    else if (rnd < 0.7f) cnt = 4;
                    else if (rnd < 0.85f) cnt = 5;
                    else cnt = 6;

                    SpawnPickups(cnt, position);
                }
                else if (rate < 0.88f)
                {
                    //spawn trinket
                    CustomDebugger.Log("spawn trinket");
                }
                else if (rate < 0.89f)
                {
                    //spawn normal chest
                    var chest = ObjectPoolManager.Release(PickupPool.PrefabChest, position, Quaternion.identity, new Vector3(0.7f, 0.7f, 0.7f)).GetComponent<Chest>();
                    chest.SetType(ChestType.Normal);
                }
                else if (rate < 0.9f)
                {
                    //spawn golden chest
                    var chest = ObjectPoolManager.Release(PickupPool.PrefabChest, position, Quaternion.identity, new Vector3(0.7f, 0.7f, 0.7f)).GetComponent<Chest>();
                    chest.SetType(ChestType.Locked);
                }
                else
                {
                    //spawn card
                    ObjectPoolManager.Release(PickupPool.PrefabCard, position);
                }
                break;
            case ChestType.Red:
                if (rate < 0.1f)
                {
                    //spawn red chest item
                    CustomDebugger.Log("spawn red chest item");
                }
                else if (rate < 0.15f)
                {
                    //teleport to devil/angel room
                    CustomDebugger.Log("teleport to devil/angel room");
                }
                else if (rate < 0.28f)
                {
                    //spawn 2 spiders
                    CustomDebugger.Log("spawn 2 spiders");
                }
                else if (rate < 0.41f)
                {
                    //spawn 2 mega troll bombs
                    var bomb = ObjectPoolManager.Release(PickupPool.PrefabBomb, position).GetComponent<Bomb>();
                    bomb.SetType(BombSO.BombType.MegaTroll);
                    ObjectPoolManager.Release(bomb.gameObject, position);
                }
                else if (rate < 0.55f)
                {
                    //spawn 3 blue flies
                    CustomDebugger.Log("spawn 3 blue flies");
                }
                else if (rate < 0.625f)
                {
                    //spawn 1 blue heart
                    var heart = ObjectPoolManager.Release(PickupPool.PrefabHeart, position).GetComponent<Heart>();
                    heart.SetType(HeartSO.HeartType.SoulFull);
                }
                else if (rate < 0.7f)
                {
                    //spawn 2 troll bombs
                    var bomb = ObjectPoolManager.Release(PickupPool.PrefabBomb, position).GetComponent<Bomb>();
                    bomb.SetType(BombSO.BombType.Troll);
                    ObjectPoolManager.Release(bomb.gameObject, position);
                }
                else
                {
                    //spawn 2 pills
                    ObjectPoolManager.Release(PickupPool.PrefabPill, position);
                    ObjectPoolManager.Release(PickupPool.PrefabPill, position);
                }
                break;
            default:
                break;
        }
    }

    private void SpawnPickups(int pickupCount, Vector3 position)
    {
        while (pickupCount > 0)
        {
            pickupCount--;
            var gameObject = SelectPickup();
            if (gameObject == PickupPool.PrefabCoin)
            {
                var rnd = UnityEngine.Random.value;
                var cnt = 0;
                if (rnd < 0.5f) cnt = 1;
                else if (rnd < 0.8f) cnt = 2;
                else cnt = 3;
                while (cnt > 0)
                {
                    cnt--;
                    ObjectPoolManager.Release(gameObject, position);
                }
            }
            else
                ObjectPoolManager.Release(gameObject, position);
        }
    }
    private GameObject SelectPickup()
    {
        var rnd = UnityEngine.Random.value;

        if (rnd < 0.35f) return PickupPool.PrefabCoin;
        else if (rnd < 0.55f) return PickupPool.PrefabHeart;
        else if (rnd < 0.7f) return PickupPool.PrefabKey;
        else return PickupPool.PrefabBomb;
    }
    public ChestType GenerateType()
    {
        return (ChestType)UnityEngine.Random.Range(0, ChestSprites.Length);
    }
}
