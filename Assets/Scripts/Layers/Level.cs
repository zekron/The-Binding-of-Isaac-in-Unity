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

    private void CreateRooms(Queue<MapRoomInfo> number)
    {
        Room lastRoom;
        for (; number.Count > 0;)
        {
            lastRoom = CreateRoom(number.Dequeue());
        }
        currentRoom = roomArray[roomOffsetPoint.x, roomOffsetPoint.y];
    }

    private Room CreateRoom(MapRoomInfo roomInfo)
    {
        int x = (int)roomInfo.Coordinate.x - roomOffsetPoint.x;
        int y = (int)roomInfo.Coordinate.y - roomOffsetPoint.y;
        var result = ObjectPoolManager.Release(roomPrefab,
                                               GameLogicUtility.LocalPointToWorldPoint(transform,
                                                                                       new Vector3(x * Room.RoomWidth,
                                                                                                   y * Room.RoomHeight)),
                                               Quaternion.identity,
                                               transform).GetComponent<Room>();
        result.coordinate = roomInfo.CoordinateVector;

        return result;
    }
}
