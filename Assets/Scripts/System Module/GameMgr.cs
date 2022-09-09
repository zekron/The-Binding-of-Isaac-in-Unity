using AssetBundleFramework;
using UnityEngine;

public class GameMgr : PersistentSingleton<GameMgr>
{
    [SerializeField] private PlayerProfileTreeAsset playerProfileAsset;

    public PlayerProfileTreeAsset PlayerProfileAsset
    {
        get
        {
            if (playerProfileAsset == null)
                playerProfileAsset = AssetBundleManager.Instance.LoadAsset<PlayerProfileTreeAsset>(StaticData.FILE_PLAYERPROFILE_SO);
            return playerProfileAsset;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        //characterProfileBundle = ResourcesLoader.LoadAssetBundleAtPath(string.Format("{0}/AssetBundles/characterprofile.ab", Application.streamingAssetsPath));

        //var prefab = AssetBundleManager.Instance.LoadAsset<GameObject>("Pedestal.prefab");
        //Instantiate(prefab);
        //Viewport.Initialize();
    }

    public PlayerProfileTreeElement GetPlayerProfileByID(int ID) => PlayerProfileAsset.GetProfileByID(ID);
}
