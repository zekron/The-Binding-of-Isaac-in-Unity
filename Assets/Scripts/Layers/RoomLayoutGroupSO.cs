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

    public RoomLayoutSO GetRoomLayoutByType(RoomObjectType type)
    {
        switch (type)
        {
            case RoomObjectType.Starting:
                return StartingRoomLayout;
            case RoomObjectType.Normal:
                return NormalRoomLayout[Random.Range(0, NormalRoomLayout.Length)];
            case RoomObjectType.Boss:
                return BossRoomLayout[Random.Range(0, BossRoomLayout.Length)];
            case RoomObjectType.MiniBoss:
                break;
            case RoomObjectType.Devil:
                break;
            case RoomObjectType.Angel:
                break;
            case RoomObjectType.Treasure:
                return TreasureRoomLayout[Random.Range(0, TreasureRoomLayout.Length)];
            case RoomObjectType.Shop:
                return ShopRoomLayout[Random.Range(0, ShopRoomLayout.Length)];
            case RoomObjectType.Library:
                break;
            case RoomObjectType.Arcade:
                break;
            case RoomObjectType.Challenge:
                break;
            case RoomObjectType.BossChallenge:
                break;
            case RoomObjectType.Curse:
                break;
            case RoomObjectType.Sacrifice:
                break;
            case RoomObjectType.Secret:
                return SecretRoomLayout[Random.Range(0, SecretRoomLayout.Length)];
            case RoomObjectType.SuperSecret:
                return SuperSecretRoomLayout[Random.Range(0, SuperSecretRoomLayout.Length)];
            case RoomObjectType.Error:
                break;
            default:
                break;
        }
        return EmptyRoomLayout;
    }
}
