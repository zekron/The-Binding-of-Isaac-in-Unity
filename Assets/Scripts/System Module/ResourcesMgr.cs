using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourcesMgr
{
#if UNITY_EDITOR
    private static readonly string[] roomLayoutAssetPath = new[]
    {
        "Assets/ScriptableObjects/RoomLayout/NormalRoom/N-01-Dip4.asset",
        "Assets/ScriptableObjects/RoomLayout/BossRoom/B-01-DukeOfFlies.asset",
        "Assets/ScriptableObjects/RoomLayout/TreasureRoom/T-01-Fireplace.asset",
        "Assets/ScriptableObjects/RoomLayout/ShopRoom/S-01.asset",
        "Assets/ScriptableObjects/RoomLayout/TestRoom/T-Start.asset",
    };
    public static string GetNormalRoomAsset() => roomLayoutAssetPath[0];
    public static string GetBossRoomAsset() => roomLayoutAssetPath[1];
    public static string GetTreasureRoomAsset() => roomLayoutAssetPath[2];
    public static string GetShopRoomAsset() => roomLayoutAssetPath[3];
    public static string GetTestRoomAsset() => roomLayoutAssetPath[4];

    public static T LoadAssetAtPath<T>(string path) where T : UnityEngine.Object
    {
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
#endif

    public static string GetNormalRoomPath() => StaticData.RoomLayoutFolderPath[0];
    public static string GetBossRoomPath() => StaticData.RoomLayoutFolderPath[1];
    public static string GetTreasureRoomPath() => StaticData.RoomLayoutFolderPath[2];
    public static string GetShopRoomPath() => StaticData.RoomLayoutFolderPath[3];


    private static Dictionary<string, AssetBundle> assetBundleCache = new Dictionary<string, AssetBundle>();
    public static AssetBundle LoadAssetBundleAtPath(string fileFullPath)
    {
        if (File.Exists(fileFullPath))
        {
            if (!assetBundleCache.ContainsKey(fileFullPath))
            {
                assetBundleCache.Add(fileFullPath, AssetBundle.LoadFromFile(fileFullPath));
            }

            return assetBundleCache[fileFullPath];
        }
        else
        {
            CustomDebugger.ThrowException(string.Format("Path {0} not exist.", fileFullPath));
            return null;
        }
    }

    public static void UnloadAllAssetBundles(bool uploadAllObjects)
    {
        AssetBundle.UnloadAllAssetBundles(uploadAllObjects);
    }
}
