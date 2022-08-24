using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : PersistentSingleton<GameMgr>
{
    [SerializeField] private PlayerProfileTreeAsset playerProfileAsset;

    private AssetBundle characterProfileBundle;

    protected override void Awake()
    {
        base.Awake();

        characterProfileBundle = ResourcesLoader.LoadAssetBundleAtPath(string.Format("{0}/AssetBundles/characterprofile.ab", Application.streamingAssetsPath));
        playerProfileAsset = characterProfileBundle.LoadAsset<PlayerProfileTreeAsset>("PlayerProfile TreeAsset");
        //Viewport.Initialize();
    }

    private void OnEnable()
    {
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

    public PlayerProfileTreeElement GetPlayerProfileByID(int ID) => playerProfileAsset.GetProfileByID(ID);
}
