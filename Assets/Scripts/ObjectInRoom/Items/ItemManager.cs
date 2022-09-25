using AssetBundleFramework;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ItemManager
{
    private static ItemManager _Instance;

    private static CollectibleItemTreeAsset collectibleItemAsset;
    private static TrinketItemTreeAsset trinketItemAsset;
    public static ObjectPool[] collectibleItemPool;
    public static ObjectPool[] trinketPool;
    private ItemManager()
    {
        collectibleItemAsset = AssetBundleManager.Instance.LoadAsset<CollectibleItemTreeAsset>(StaticData.FILE_COLLECTIBLEITEM_SO);
        collectibleItemPool = InstantiatePools(collectibleItemAsset);
        trinketItemAsset = AssetBundleManager.Instance.LoadAsset<TrinketItemTreeAsset>(StaticData.FILE_TRINKETITEM_SO);
        trinketPool = InstantiatePools(trinketItemAsset);
    }

    private static void ItemPoolInitialize()
    {
        ObjectPoolManager.Initialize(collectibleItemPool);
        ObjectPoolManager.Initialize(trinketPool);
    }

    public static ItemManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ItemManager();
                ItemPoolInitialize();
            }
            return _Instance;
        }
    }

    private static ObjectPool[] InstantiatePools<T>(ItemTreeAsset<T> itemTreeAsset) where T : ItemTreeElement
    {
        List<ObjectPool> pools = new List<ObjectPool>();
        for (int i = 0; i < itemTreeAsset.TreeElements.Count; i++)
        {
            if (itemTreeAsset.TreeElements[i].ItemPrefab != null)
                pools.Add(new ObjectPool(itemTreeAsset.TreeElements[i].ItemPrefab));
        }

        return pools.ToArray();
    }

    public int GetCollectibleItemCount() => collectibleItemAsset.TreeCount;
    public int TrinketItemCount() => trinketItemAsset.TreeCount;

    public CollectibleItemTreeElement GetCollectibleItemProfileByID(int ID) => collectibleItemAsset.GetProfileByID(ID);
    public ItemTreeElement GetTrinketItemProfileByID(int ID) => trinketItemAsset.GetProfileByID(ID);
}
