using AssetBundleFramework;
using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间相关
/// </summary>
public class Room : MonoBehaviour
{
    [Header("房间属性")]
    public MapRoomInfo RoomInfo;

    [SerializeField] private RoomLayoutSO roomLayout;//布局文件

    public bool isArrived = false;//是否已到达
    public bool isCleared = false;//是否已清理过

    #region 房间布局
    [Header("房间布局")]
    [SerializeField] private SpriteRenderer[] topBottomWallSprite;
    [SerializeField] private SpriteRenderer[] leftRightWallSprite;
    [SerializeField] private SpriteRenderer floorSprite;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform layoutTransform;
    [SerializeField] private GameObject[] doorLocks;
    private List<Door> roomDoors;
    private int doorsCount => roomDoors.Count;
    #endregion

    [Header("Events")]
    [SerializeField] private TwoVector3EventChannelSO onEnterRoomEvent;
    [SerializeField] private VoidEventChannelSO onClearRoomEvent;

    private void OnEnable()
    {
        roomLayout = AssetBundleManager.Instance.LoadAsset<RoomLayoutSO>("Starting RoomLayout.asset");
        RoomLayoutInitialize();
        if (roomDoors == null)
        {
            roomDoors = new List<Door>(4);
        }
    }

    private void OnDisable()
    {

    }

    private void Start()
    {

    }

    private void RoomLayoutInitialize()
    {
        floorSprite.sprite = roomLayout.SpriteFloor;
        for (int i = 0; i < topBottomWallSprite.Length; i++)
        {
            topBottomWallSprite[i].sprite = roomLayout.SpriteTop;
            leftRightWallSprite[i].sprite = roomLayout.SpriteLeft;
        }

        for (int i = 0; i < roomLayout.obstacleList.Count; i++)
        {
            var obstacle = ObjectPoolManager.Release(
                    roomLayout.obstacleList[i].value1,
                    roomLayout.obstacleList[i].value2.ToRoomCoordinate().ToRoomPosition().ToWorldPosition(transform),
                    Quaternion.identity,
                    layoutTransform).GetComponent<IObjectInRoom>();
            if (obstacle == null) obstacle = GetComponentInChildren<IObjectInRoom>();
            
            obstacle.Coordinate = roomLayout.obstacleList[i].value2;
            obstacle.ChangeRendererOrder();
        }
        for (int i = 0; i < roomLayout.propList.Count; i++)
        {
            var prop = ObjectPoolManager.Release(
                    roomLayout.propList[i].value1.GenerateObject(),
                    roomLayout.propList[i].value2.ToRoomCoordinate().ToRoomPosition().ToWorldPosition(transform),
                    Quaternion.identity,
                    layoutTransform).GetComponentInChildren<IObjectInRoom>();

            prop.Coordinate = roomLayout.propList[i].value2;
            prop.ChangeRendererOrder();
        }
    }

    public void SetRoomLayout(RoomLayoutSO roomLayout)
    {
        this.roomLayout = roomLayout;

        RoomLayoutInitialize();
    }

    public void EnterRoom(DoorPosition doorPosition)
    {
        var playerPosition = transform.position;
        switch (doorPosition)
        {
            case DoorPosition.Up:
                playerPosition.y -= 2.84f;
                break;
            case DoorPosition.Down:
                playerPosition.y += 2.84f;
                break;
            case DoorPosition.Left:
                playerPosition.x += 5.64f;
                break;
            case DoorPosition.Right:
                playerPosition.x -= 5.64f;
                break;
            default:
                break;
        }
        onEnterRoomEvent.RaiseEvent(transform.localPosition, playerPosition);

        isCleared = true;
        if (isCleared)
        {
            foreach (var door in roomDoors)
            {
                if (door.DoorType == DoorObjectType.Secret &&
                    (RoomInfo.CurrentRoomType != RoomObjectType.Secret || RoomInfo.CurrentRoomType != RoomObjectType.SuperSecret)) continue;

                doorLocks[(int)door.doorPosition].SetActive(false);
                door.RaiseEvent(DoorStatus.Open);
            }
        }
    }

    public void CreateDoor(GameCoordinate direction, RoomObjectType roomType = RoomObjectType.Normal)
    {
        var tempTransform = Door.GetDoorTransform(direction.ToDoorPosition());

        roomDoors.Add(CustomObjectPoolManager.Release(GetDoorTypeWithRoomType(roomType),
                                                tempTransform.localPosition.ToWorldPosition(transform),
                                                tempTransform.rotation,
                                                doorTransform).GetComponent<Door>());

        if (roomType == RoomObjectType.Secret || roomType == RoomObjectType.SuperSecret) roomDoors[roomDoors.Count - 1].gameObject.SetActive(false);
        roomDoors[doorsCount - 1].doorPosition = direction.ToDoorPosition();
        //test
        //roomDoors[doorsCount - 1].RaiseEvent(DoorStatus.Open);
        //onDoorStatusChanged.RaiseEvent(DoorStatus.Open);
    }

    private DoorObjectType GetDoorTypeWithRoomType(RoomObjectType type)
    {
        switch (type)
        {
            case RoomObjectType.Starting:
            case RoomObjectType.Normal:
            case RoomObjectType.MiniBoss:
            case RoomObjectType.Error:
            case RoomObjectType.Library:
            case RoomObjectType.Sacrifice:
                return DoorObjectType.Normal;
            case RoomObjectType.Boss:
                return DoorObjectType.Boss;
            case RoomObjectType.Devil:
                return DoorObjectType.Devil;
            case RoomObjectType.Angel:
                return DoorObjectType.Angel;
            case RoomObjectType.Treasure:
                return DoorObjectType.Treasure;
            case RoomObjectType.Shop:
                return DoorObjectType.Shop;
            case RoomObjectType.Arcade:
                return DoorObjectType.Arcade;
            case RoomObjectType.Challenge:
                return DoorObjectType.Challenge;
            case RoomObjectType.BossChallenge:
                return DoorObjectType.BossChallenge;
            case RoomObjectType.Curse:
                return DoorObjectType.Curse;
            case RoomObjectType.Secret:
            case RoomObjectType.SuperSecret:
                return DoorObjectType.Secret;
        }
        return DoorObjectType.Normal;
    }
}
