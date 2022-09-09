using AssetBundleFramework;
using System.Collections.Generic;

public class ItemManager
{
    private static ItemManager _Instance;

    private static CollectibleItemTreeAsset collectibleItemAsset;
    private static TrinketItemTreeAsset trinketItemAsset;

    private ItemManager()
    {
        collectibleItemAsset = AssetBundleManager.Instance.LoadAsset<CollectibleItemTreeAsset>(StaticData.FILE_COLLECTIBLEITEM_SO);
        trinketItemAsset = AssetBundleManager.Instance.LoadAsset<TrinketItemTreeAsset>(StaticData.FILE_TRINKETITEM_SO);
    }

    public static ItemManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ItemManager();

                ObjectPoolManager.Initialize(InstantiatePools(collectibleItemAsset));
                ObjectPoolManager.Initialize(InstantiatePools(trinketItemAsset));
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
