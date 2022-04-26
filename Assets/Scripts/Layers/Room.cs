using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间相关
/// </summary>
public class Room : MonoBehaviour
{
    [Header("房间属性")]

    [HideInInspector]
    public RoomType roomType;//房间类型

    [HideInInspector]
    public Vector2 coordinate;//坐标

    public MapRoomInfo RoomInfo;

    [SerializeField] private RoomLayoutSO roomLayout;//布局文件

    public bool isArrived = false;//是否已到达
    public bool isCleared = false;//是否已清理过

    //房间宽高，像素值/100
    public static float RoomWidth { get { return 15.06f; } }
    public static float RoomHeight { get { return 9.44f; } }
    public static int RoomWidthPixels { get { return 1506; } }
    public static int RoomHeightPixels { get { return 944; } }

    //单位数量和大小
    public static int HorizontalUnit { get { return 13; } }
    public static int VerticalUnit { get { return 7; } }
    public static float UnitSize { get { return 0.28f; } }

    #region 房间布局
    [SerializeField] private SpriteRenderer[] topBottomWallSprite;
    [SerializeField] private SpriteRenderer[] leftRightWallSprite;
    [SerializeField] private SpriteRenderer floorSprite;
    [SerializeField] private Transform doorTransform;
    private List<Door> roomDoors;
    private int doorsCount => roomDoors.Count;
    #endregion

    #region Test
    [Header("Test Part")]
    [SerializeField] private GameObject doorPrefab;
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
        //DoorInitialize();
    }

    private void RoomLayoutInitialize()
    {
        floorSprite.sprite = roomLayout.SpriteFloor;
        for (int i = 0; i < topBottomWallSprite.Length; i++)
        {
            topBottomWallSprite[i].sprite = roomLayout.SpriteTop;
            leftRightWallSprite[i].sprite = roomLayout.SpriteLeft;
        }
    }

    //private void DoorInitialize()
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        var tempTransform = Door.GetDoorTransform((DoorPosition)i);
    //        roomDoors.Add(ObjectPoolManager.Release(doorPrefab,
    //                                                GameLogicUtility.LocalPointToWorldPoint(transform, tempTransform.localPosition),
    //                                                tempTransform.rotation,
    //                                                doorTransform).GetComponent<Door>());
    //        roomDoors[i].RaiseEvent(DoorStatus.Open);
    //    }
    //}

    public void CreateDoor(Vector2 vector,RoomType roomType = RoomType.Normal)
    {
        (Vector3 localPosition, Quaternion rotation) tempTransform = (Vector3.zero, Quaternion.identity);
        if (vector == Vector2.up)
        {
            tempTransform = Door.GetDoorTransform(DoorPosition.Up);
        }
        else if (vector == Vector2.down)
        {
            tempTransform = Door.GetDoorTransform(DoorPosition.Down);
        }
        else if (vector == Vector2.left)
        {
            tempTransform = Door.GetDoorTransform(DoorPosition.Left);
        }
        else if (vector == Vector2.right)
        {
            tempTransform = Door.GetDoorTransform(DoorPosition.Right);
        }
        roomDoors.Add(ObjectPoolManager.Release(doorPrefab,
                                                GameLogicUtility.LocalPointToWorldPoint(transform,
                                                                                        tempTransform.localPosition),
                                                tempTransform.rotation,
                                                doorTransform).GetComponent<Door>());
        roomDoors[doorsCount - 1].RaiseEvent(DoorStatus.Open);
    }
}
