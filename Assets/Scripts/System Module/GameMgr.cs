using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : PersistentSingleton<GameMgr>
{
    [SerializeField] private PlayerProfileTreeAsset playerProfileAssets;

    // Start is called before the first frame update
    void OnEnable()
    {
        var assetBundle = ResourcesMgr.LoadAssetBundleAtPath(string.Format("{0}/AssetBundles/characterprofile.ab", Application.streamingAssetsPath));
        playerProfileAssets = assetBundle.LoadAsset<PlayerProfileTreeAsset>("PlayerProfile TreeAsset");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public PlayerProfileTreeElement GetPlayerProfileByID(int ID)
    {
        return playerProfileAssets.GetProfileByID(ID);
    }
}
