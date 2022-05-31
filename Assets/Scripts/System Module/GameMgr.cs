using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : PersistentSingleton<GameMgr>
{
    [SerializeField] private PlayerProfileTreeAsset playerProfileAssets;

    void OnEnable()
    {
        var assetBundle = ResourcesMgr.LoadAssetBundleAtPath(string.Format("{0}/AssetBundles/characterprofile.ab", Application.streamingAssetsPath));
        playerProfileAssets = assetBundle.LoadAsset<PlayerProfileTreeAsset>("PlayerProfile TreeAsset");

        //Viewport.Initialize();
    }

    void Update()
    {

    }

    public PlayerProfileTreeElement GetPlayerProfileByID(int ID)
    {
        return playerProfileAssets.GetProfileByID(ID);
    }
}
