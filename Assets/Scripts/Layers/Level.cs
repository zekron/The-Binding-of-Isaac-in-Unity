using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    private Room[,] roomArray = new Room[20, 20];
    private Room currentRoom;
    private static readonly (int x, int y) roomOffsetPoint = (10, 10);

    // Start is called before the first frame update
    void Start()
    {
        CreateRooms(MapGenerator.CreateMap(ChapterType.Basement, 1, FloorCurseType.None, isHardMode: false));
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

    private void CreateRooms(MapRoomInfo map)
    {
        Queue<MapRoomInfo> queue = new Queue<MapRoomInfo>();
        queue.Enqueue(map);

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
        currentRoom = roomArray[roomOffsetPoint.x, roomOffsetPoint.y];
    }

    private Room CreateRoom(MapRoomInfo roomInfo)
    {
        int x = roomInfo.Coordinate.x - roomOffsetPoint.x;
        int y = roomInfo.Coordinate.y - roomOffsetPoint.y;
        var result = ObjectPoolManager.Release(roomPrefab,
                                               GameLogicUtility.LocalPointToWorldPoint(transform,
                                                                                       new Vector3(x * Room.RoomWidth,
                                                                                                   y * Room.RoomHeight,
                                                                                                   roomInfo.Depth)),
                                               Quaternion.identity,
                                               transform).GetComponent<Room>();
        result.RoomInfo = roomInfo;

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
        if (coordinate == MapCoordinate.up)
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.up, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.down, currentRoom.RoomInfo.CurrentRoomType);
        }
        else if (coordinate == MapCoordinate.down)
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.down, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.up, currentRoom.RoomInfo.CurrentRoomType);
        }
        else if (coordinate == MapCoordinate.left)
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.left, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.right, currentRoom.RoomInfo.CurrentRoomType);
        }
        else if (coordinate == MapCoordinate.right)
        {
            roomArray[parentInfo.Coordinate.x,
                      parentInfo.Coordinate.y].CreateDoor(Vector2.right, currentRoom.RoomInfo.CurrentRoomType);
            currentRoom.CreateDoor(Vector2.left, currentRoom.RoomInfo.CurrentRoomType);
        }
    }
}
