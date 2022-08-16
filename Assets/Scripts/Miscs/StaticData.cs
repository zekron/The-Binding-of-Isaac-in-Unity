using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData
{
    public static readonly Vector3 ViewportTopRight = new Vector3(0.92f, 0.765f);
    public static readonly Vector3 ViewportBottomLeft = new Vector3(0.08f, 0.155f);

    #region Room
    //房间宽高，像素值/100
    public static readonly float RoomWidth = 15.06f;
    public static readonly float RoomHeight = 9.44f;
    public static readonly int RoomWidthPixels = 1506;
    public static readonly int RoomHeightPixels = 944;

    //单位数量和大小
    public const int ROOM_EDITOR_WINDOW_MAX_X = 25;
    public const int ROOM_EDITOR_WINDOW_MAX_Y = 13;
    public static readonly int RoomHorizontalUnit = 13;
    public static readonly int RoomVerticalUnit = 7;
    public static readonly float RoomHorizontalUnitSize = 0.94f;
    public static readonly float RoomVerticalUnitSize = 0.95f;
    #endregion

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
    public static readonly string FILE_TRINKETITEM_SO = "TrinketItem TreeAsset.asset";
    public static readonly string FILE_COLLECTIBLEITEM_SO = "CollectibleItem TreeAsset.asset";
    #endregion
}
