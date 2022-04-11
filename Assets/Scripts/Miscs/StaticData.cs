using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData
{
    public static readonly Vector3 ViewportTopRight = new Vector3(0.92f, 0.765f);
    public static readonly Vector3 ViewportBottomLeft = new Vector3(0.08f, 0.155f);

    #region AssetPath
    public static readonly string ScriptableObjectFolderPath = "Assets/ScriptableObjects/";
    public static readonly string[] RoomLayoutFolderPath = new[]
    {
        "Assets/ScriptableObjects/RoomLayout/NormalRoom",
        "Assets/ScriptableObjects/RoomLayout/BossRoom",
        "Assets/ScriptableObjects/RoomLayout/TreasureRoom",
        "Assets/ScriptableObjects/RoomLayout/ShopRoom",
    };
    #endregion

    #region AssetName
    public static readonly string FILE_ENEMYPROFILE_SO = "EnemyProfile TreeAsset.asset";
    public static readonly string FILE_PLAYERPROFILE_SO = "PlayerProfile TreeAsset.asset";
    #endregion
}
