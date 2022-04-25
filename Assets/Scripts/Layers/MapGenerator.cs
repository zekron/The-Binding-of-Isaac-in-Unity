using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    private static readonly MapCoordinate roomOffsetPoint = new MapCoordinate(10, 10);
    private static List<MapCoordinate> coordinateList = new List<MapCoordinate>();
    private static Queue<MapRoomInfo> coordinateQueue = new Queue<MapRoomInfo>();

    public static MapRoomInfo CreateMap(ChapterType chapterType, int floorDepth, FloorCurseType curseType, bool isHardMode)
    {
        coordinateList.Clear();
        coordinateQueue.Clear();

        //生成初始房间地图
        var roomNumber = GetRoomNumberWithLevelFloorDepth(floorDepth, curseType, isHardMode);
        var result = new MapRoomInfo(roomOffsetPoint, null);
        var parent = result;
        coordinateQueue.Enqueue(result);
        roomNumber--;

        while (coordinateQueue.Count > 0 && roomNumber > 0)
        {
            roomNumber -= GenerateRoomCoordinate(coordinateQueue.Dequeue());
        }

        //TODO: 获得死路房间坐标，设置特殊房间
        var deadEnds = GetMinDeadEndsWithFloorDepth(floorDepth, curseType);

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
    private static int GenerateRoomCoordinate(MapRoomInfo curRoomInfo)
    {
        Queue<MoveDirection> directionQueue = new Queue<MoveDirection>();
        int result = 0;

        for (int i = 0; i < System.Enum.GetValues(typeof(MoveDirection)).Length; i++)
        {
            if (CanGenerate(curRoomInfo.Coordinate, (MoveDirection)i))
            {
                directionQueue.Enqueue((MoveDirection)i);
            }
        }

        //Try Generate
        var queueCount = directionQueue.Count;
        for (int i = 1; directionQueue.Count > 0; i++)
        {
            var direction = directionQueue.Dequeue();
            if (Random.value <= Percent(queueCount, queueCount - i))
            {
                var newPoint = new MapRoomInfo(curRoomInfo.Coordinate + GetMoveDirectionPoint(direction), curRoomInfo);
                coordinateQueue.Enqueue(newPoint);
                curRoomInfo.Children.Add(newPoint);
                result++;
            }
        }

        return result;
    }

    private static bool CanGenerate(MapCoordinate point, MoveDirection direction)
    {
        return !coordinateList.Contains(point + GetMoveDirectionPoint(direction));
    }

    private static float Percent(int total, int remain)
    {
        return (float)(total - remain) / total;
        //if (remain == 0) return 1;
        //{

        //}
        //return 0.7f;
    }

    private static MapCoordinate GetMoveDirectionPoint(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return MapCoordinate.up;
            case MoveDirection.Down:
                return MapCoordinate.down;
            case MoveDirection.Left:
                return MapCoordinate.left;
            case MoveDirection.Right:
                return MapCoordinate.right;
            default:
                return MapCoordinate.zero;
        }
    }

    enum MoveDirection { Up, Down, Left, Right }
}
