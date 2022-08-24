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

    #region Test
    [Header("Test Part")]
    [SerializeField] private GameObject normalDoor;
    [SerializeField] private GameObject bossDoor;
    [SerializeField] private GameObject treasureDoor;
    [SerializeField] private GameObject shopDoor;
    [SerializeField] private GameObject secretDoor;
    [SerializeField] private GameObject arcadeDoor;
    [SerializeField] private GameObject bossChallengeDoor;
    [SerializeField] private GameObject challengeDoor;
    [SerializeField] private GameObject curseDoor;
    #endregion

    private void OnEnable()
    {
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
                if (door.DoorType == DoorType.Secret &&
                    (RoomInfo.CurrentRoomType != RoomType.Secret || RoomInfo.CurrentRoomType != RoomType.SuperSecret)) continue;

                doorLocks[(int)door.doorPosition].SetActive(false);
                door.RaiseEvent(DoorStatus.Open);
            }
        }
    }

    public void CreateDoor(GameCoordinate direction, RoomType roomType = RoomType.Normal)
    {
        var tempTransform = Door.GetDoorTransform(direction.ToDoorPosition());

        roomDoors.Add(ObjectPoolManager.Release(GetDoorPrefabWithRoomType(roomType),
                                                tempTransform.localPosition.ToWorldPosition(transform),
                                                tempTransform.rotation,
                                                doorTransform).GetComponent<Door>());

        if (roomType == RoomType.Secret || roomType == RoomType.SuperSecret) roomDoors[roomDoors.Count - 1].gameObject.SetActive(false);
        roomDoors[doorsCount - 1].doorPosition = direction.ToDoorPosition();
        //test
        //roomDoors[doorsCount - 1].RaiseEvent(DoorStatus.Open);
        //onDoorStatusChanged.RaiseEvent(DoorStatus.Open);
    }

    private GameObject GetDoorPrefabWithRoomType(RoomType type)
    {
        switch (type)
        {
            case RoomType.Starting:
            case RoomType.Normal:
            case RoomType.MiniBoss:
            case RoomType.Error:
            case RoomType.Library:
            case RoomType.Sacrifice:
                return normalDoor;
            case RoomType.Boss:
                return bossDoor;
            case RoomType.Devil:
                break;
            case RoomType.Angel:
                break;
            case RoomType.Treasure:
                return treasureDoor;
            case RoomType.Shop:
                return shopDoor;
            case RoomType.Arcade:
                return arcadeDoor;
            case RoomType.Challenge:
                return challengeDoor;
            case RoomType.BossChallenge:
                return bossChallengeDoor;
            case RoomType.Curse:
                return curseDoor;
            case RoomType.Secret:
            case RoomType.SuperSecret:
                return secretDoor;
        }
        return normalDoor;
    }
}
