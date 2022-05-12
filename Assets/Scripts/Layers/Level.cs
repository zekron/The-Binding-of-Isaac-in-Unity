using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private FloorCurseType currentCurse = FloorCurseType.None;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private MapCoordinateEventChannelSO onCreateRoomEvent;
    [SerializeField] private MapCoordinateStatusEventChannelSO onEnterRoomEvent;

    private Room[,] roomArray = new Room[MapCoordinate.RoomOffsetPoint.x << 1, MapCoordinate.RoomOffsetPoint.y << 1];
    private Room currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        CreateRooms(MapGenerator.CreateMap(ChapterType.Basement, currentLevel, currentCurse, isHardMode: false));

        onEnterRoomEvent.RaiseEvent(MapCoordinate.RoomOffsetPoint, MiniMapIconStatus.Current);
        currentRoom = roomArray[MapCoordinate.RoomOffsetPoint.x, MapCoordinate.RoomOffsetPoint.y];
        //CreateRooms(Random.Range(1, 5));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void CreateRooms(MapRoomInfo rootRoomInfo)
    {
        Queue<MapRoomInfo> queue = new Queue<MapRoomInfo>();
        queue.Enqueue(rootRoomInfo);

        while (queue.Count > 0)
        {
            var info = queue.Dequeue();

            currentRoom = CreateRoom(info);
            roomArray[currentRoom.RoomInfo.Coordinate.x, currentRoom.RoomInfo.Coordinate.y] = currentRoom;
            CreateDoor(currentRoom);

            foreach (var child in info.Children)
            {
                queue.Enqueue(child);
            }
        }
        //for (; map.Count > 0;)
        //{
        //    currentRoom = CreateRoom(map.Dequeue());
        //    roomArray[currentRoom.RoomInfo.Coordinate.x, currentRoom.RoomInfo.Coordinate.y] = currentRoom;

        //    CreateDoor(currentRoom);
        //    lastRoom = currentRoom;
        //}
    }

    private void EnterRoom(MapCoordinate coordinate)
    {
        onEnterRoomEvent.RaiseEvent(MapCoordinate.zero,
                                    currentRoom.isCleared ? MiniMapIconStatus.Explored : MiniMapIconStatus.Unexplored);

        onEnterRoomEvent.RaiseEvent(coordinate - currentRoom.RoomInfo.Coordinate,
                                    MiniMapIconStatus.Current);
        currentRoom = roomArray[coordinate.x, coordinate.y];
    }

    private Room CreateRoom(MapRoomInfo roomInfo)
    {
        var coordinate = roomInfo.Coordinate - MapCoordinate.RoomOffsetPoint;
        var result = ObjectPoolManager.Release(roomPrefab,
                                               GameLogicUtility.LocalPointToWorldPoint(transform,
                                                                                       new Vector3(coordinate.x * Room.RoomWidth,
                                                                                                   coordinate.y * Room.RoomHeight,
                                                                                                   roomInfo.Depth)),
                                               Quaternion.identity,
                                               transform).GetComponent<Room>();
        result.RoomInfo = roomInfo;
        onCreateRoomEvent.RaiseEvent(roomInfo.Coordinate);

        return result;
    }

    /// <summary>
    /// 检查 <paramref name="currentRoom"></paramref> 四个方向是否有房间，有则创建门
    /// </summary>
    /// <param name="currentRoom"></param>
    /// <param name="lastRoom"></param>
    private void CreateDoor(Room currentRoom)
    {
        if (currentRoom == null || currentRoom.RoomInfo.Parent == null) return;

        MapRoomInfo parentInfo = currentRoom.RoomInfo.Parent;
        var coordinate = currentRoom.RoomInfo.Coordinate - parentInfo.Coordinate;
        if (coordinate.Equals(MapCoordinate.up))
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.up, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.down, currentRoom.RoomInfo.CurrentRoomType);
        }
        else if (coordinate.Equals(MapCoordinate.down))
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.down, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.up, currentRoom.RoomInfo.CurrentRoomType);
        }
        else if (coordinate.Equals(MapCoordinate.left))
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.left, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.right, currentRoom.RoomInfo.CurrentRoomType);
        }
        else if (coordinate.Equals(MapCoordinate.right))
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.right, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.left, currentRoom.RoomInfo.CurrentRoomType);
        }
    }
}
