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
        coordinateList.Add(roomOffsetPoint);
        roomNumber--;

        while (coordinateQueue.Count > 0 && roomNumber > 0)
        {
            GenerateRoomCoordinate(coordinateQueue.Dequeue(), ref roomNumber);
        }
        if (roomNumber > 0)
        {

        }

        //TODO: 获得死路房间坐标，设置特殊房间
        var deadEnds = GetMinDeadEndsWithFloorDepth(floorDepth, curseType);

        return result;
    }

    /// <summary>
    /// 根据 <paramref name="curseType"/> 和 <paramref name="isHardMode"/> 计算出当前 <paramref name="floorDepth"/> 的房间数
    /// </summary>
    /// <param name="floorDepth">层深</param>
    /// <param name="curseType">诅咒类型</param>
    /// <param name="isHardMode">是否为困难模式</param>
    /// <returns></returns>
    private static int GetRoomNumberWithLevelFloorDepth(int floorDepth, FloorCurseType curseType, bool isHardMode)
    {
        int roomNumber = Mathf.Min(20, Random.Range(0, 2) + 5 + floorDepth * 10 / 3);

        if (curseType == FloorCurseType.CurseoftheLabyrinth) roomNumber = Mathf.Min(45, (int)(1.8f * roomNumber));
        else if (curseType == FloorCurseType.CurseoftheLost) roomNumber += 4;

        if (isHardMode) roomNumber += 2 + Random.Range(0, 2);

        return roomNumber;
    }

    /// <summary>
    /// 根据 <paramref name="curseType"/> 计算出当前 <paramref name="floorDepth"/> 的最少死路数
    /// </summary>
    /// <param name="floorDepth">层深</param>
    /// <param name="curseType">诅咒类型（是否受到 Curse of the Labyrinth）</param>
    /// <returns></returns>
    private static int GetMinDeadEndsWithFloorDepth(int floorDepth, FloorCurseType curseType)
    {
        var minDeadEnds = 5;

        if (floorDepth != 1) minDeadEnds += 1;
        if (curseType == FloorCurseType.CurseoftheLabyrinth) minDeadEnds += 1;

        return minDeadEnds;
    }

    private static int GenerateRoomCoordinate(MapRoomInfo curRoomInfo, ref int roomNumber)
    {
        Queue<MoveDirection> directionQueue = new Queue<MoveDirection>();

        directionArray.Shuffle();
        int length = directionArray.Length - (2 - curRoomInfo.Depth);
        for (int i = 0; i < length; i++)
        {
            if (CanGenerate(curRoomInfo.Coordinate, (MoveDirection)i))
            {
                directionQueue.Enqueue((MoveDirection)i);
            }
        }

        //Try Generate
        int result = 0;
        var queueCount = directionQueue.Count;
        for (int i = 0; directionQueue.Count > 0 && roomNumber > 0; i++)
        {
            var direction = directionQueue.Dequeue();
            if (Random.value <= Percent(queueCount, queueCount - i, result))
            {
                var newCoordinate = curRoomInfo.Coordinate + GetMoveDirectionPoint(direction);
                coordinateList.Add(newCoordinate);
                var newPoint = new MapRoomInfo(newCoordinate, curRoomInfo);
                coordinateQueue.Enqueue(newPoint);
                curRoomInfo.Children.Add(newPoint);
                result++;
                roomNumber--;
            }
        }

        return result;
    }

    private static bool CanGenerate(MapCoordinate point, MoveDirection direction)
    {
        return !coordinateList.Contains(point + GetMoveDirectionPoint(direction));
    }

    /// <summary>
    /// 根据 <paramref name="result"/> 和 <paramref name="remain"/> 情况调整成功概率
    /// </summary>
    /// <param name="total">可以生成房间的坐标数目</param>
    /// <param name="remain">剩余可以生成房间的坐标数目</param>
    /// <param name="result">当前已经可以生成房间的坐标数目</param>
    /// <returns></returns>
    private static float Percent(int total, int remain, int result)
    {
        return (float)remain / total;
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

    static MoveDirection[] directionArray = new MoveDirection[]
     {
        MoveDirection.Up,
        MoveDirection.Down,
        MoveDirection.Left,
        MoveDirection.Right
     };
    enum MoveDirection { Up, Down, Left, Right }
}
