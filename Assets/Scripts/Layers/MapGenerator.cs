using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private static readonly (int x, int y) roomOffsetPoint = (10, 10);

    public static Queue<MapRoomInfo> CreateMap(ChapterType chapterType, int floorDepth, FloorCurseType curseType, bool isHardMode)
    {
        var result = new Queue<MapRoomInfo>();
        var roomNumber = GetRoomNumberWithLevelFloorDepth(floorDepth, curseType, isHardMode);
        var deadEnds = GetMinDeadEndsWithFloorDepth(floorDepth, curseType);

        //生成初始房间地图
        result.Enqueue(new MapRoomInfo((roomOffsetPoint.x, roomOffsetPoint.y)));
        MapRoomInfo position;
        for (int i = 1; i < roomNumber; i++)
        {
            do
            {
                position = new MapRoomInfo(GenerateRoomCoordinate(result.Peek().Coordinate));
            } while (result.Contains(position));
            result.Enqueue(position);
        }
        //TODO: 获得死路房间坐标，设置特殊房间

        return result;
    }

    private static int GetRoomNumberWithLevelFloorDepth(int floorDepth, FloorCurseType curseType, bool isHardMode)
    {
        int roomNumber = Mathf.Min(20, Random.Range(0, 2) + 5 + floorDepth * 10 / 3);

        if (curseType == FloorCurseType.CurseoftheLabyrinth) roomNumber = Mathf.Min(45, (int)(1.8f * roomNumber));
        else if (curseType == FloorCurseType.CurseoftheLost) roomNumber += 4;

        if (isHardMode) roomNumber += 2 + Random.Range(0, 2);

        return roomNumber;
    }
    private static int GetMinDeadEndsWithFloorDepth(int floorDepth, FloorCurseType curseType)
    {
        var minDeadEnds = 5;

        if (floorDepth != 1) minDeadEnds += 1;
        if (curseType == FloorCurseType.CurseoftheLabyrinth) minDeadEnds += 1;

        return minDeadEnds;
    }
    private static (int x, int y) GenerateRoomCoordinate((int x, int y) curPosition)
    {
        var direction = (MoveDirection)Random.Range(0, System.Enum.GetValues(typeof(MoveDirection)).Length);
        switch (direction)
        {
            case MoveDirection.Up:
                return (curPosition.x, curPosition.y + 1);
            case MoveDirection.Down:
                return (curPosition.x, curPosition.y - 1);
            case MoveDirection.Left:
                return (curPosition.x - 1, curPosition.y);
            case MoveDirection.Right:
                return (curPosition.x + 1, curPosition.y);
            default:
                return (0, 0);
        }
    }

    enum MoveDirection { Up, Down, Left, Right }
}
