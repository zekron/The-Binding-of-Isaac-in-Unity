using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : PersistentSingleton<ItemManager>
{
    [SerializeField] private CollectibleItemTreeAsset collectibleItemAsset;
    [SerializeField] private TrinketItemTreeAsset trinketItemAsset;

    private AssetBundle characterProfileBundle;
    private AssetBundle itemsBundle;

    protected override void Awake()
    {
        base.Awake();

        itemsBundle = ResourcesLoader.LoadAssetBundleAtPath(string.Format("{0}/AssetBundles/items.ab", Application.streamingAssetsPath));
        collectibleItemAsset = itemsBundle.LoadAsset<CollectibleItemTreeAsset>(StaticData.FILE_COLLECTIBLEITEM_SO);
        trinketItemAsset = itemsBundle.LoadAsset<TrinketItemTreeAsset>(StaticData.FILE_TRINKETITEM_SO);
    }

    private void OnEnable()
    {
        ObjectPoolManager.Initialize(InstantiatePools(collectibleItemAsset));
        ObjectPoolManager.Initialize(InstantiatePools(trinketItemAsset));
    }

    private ObjectPool[] InstantiatePools<T>(ItemTreeAsset<T> itemTreeAsset) where T : ItemTreeElement
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
