using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoomLayout", menuName = "Scriptable Object/Room Layout/Room Layout Group")]
public class RoomLayoutGroupSO : ScriptableObject
{
    [SerializeField] private RoomLayoutSO[] NormalRoomLayout;
    [SerializeField] private RoomLayoutSO[] BossRoomLayout;
    [SerializeField] private RoomLayoutSO[] TreasureRoomLayout;
    [SerializeField] private RoomLayoutSO[] ShopRoomLayout;
    [SerializeField] private RoomLayoutSO[] SecretRoomLayout;
    [SerializeField] private RoomLayoutSO[] SuperSecretRoomLayout;

    [SerializeField] private RoomLayoutSO EmptyRoomLayout;
    [SerializeField] private RoomLayoutSO StartingRoomLayout;
    [SerializeField] private RoomLayoutSO DevilRoomLayout;
    [SerializeField] private RoomLayoutSO AngelRoomLayout;

    public RoomLayoutSO GetRoomLayoutByType(RoomType type)
    {
        switch (type)
        {
            case RoomType.Starting:
                return EmptyRoomLayout;
            case RoomType.Normal:
                return NormalRoomLayout[Random.Range(0, NormalRoomLayout.Length)];
            case RoomType.Boss:
                return BossRoomLayout[Random.Range(0, BossRoomLayout.Length)];
            case RoomType.MiniBoss:
                break;
            case RoomType.Devil:
                break;
            case RoomType.Angel:
                break;
            case RoomType.Treasure:
                return TreasureRoomLayout[Random.Range(0, TreasureRoomLayout.Length)];
            case RoomType.Shop:
                return ShopRoomLayout[Random.Range(0, ShopRoomLayout.Length)];
            case RoomType.Library:
                break;
            case RoomType.Arcade:
                break;
            case RoomType.Challenge:
                break;
            case RoomType.BossChallenge:
                break;
            case RoomType.Curse:
                break;
            case RoomType.Sacrifice:
                break;
            case RoomType.Secret:
                break;
            case RoomType.SuperSecret:
                break;
            case RoomType.Error:
                break;
            default:
                break;
        }
        return EmptyRoomLayout;
    }
}
