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

        //���ɳ�ʼ�����ͼ
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

        //TODO: �����·�������꣬�������ⷿ��
        var deadEnds = GetMinDeadEndsWithFloorDepth(floorDepth, curseType);

        return result;
    }

    /// <summary>
    /// ���� <paramref name="curseType"/> �� <paramref name="isHardMode"/> �������ǰ <paramref name="floorDepth"/> �ķ�����
    /// </summary>
    /// <param name="floorDepth">����</param>
    /// <param name="curseType">��������</param>
    /// <param name="isHardMode">�Ƿ�Ϊ����ģʽ</param>
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
    /// ���� <paramref name="curseType"/> �������ǰ <paramref name="floorDepth"/> ��������·��
    /// </summary>
    /// <param name="floorDepth">����</param>
    /// <param name="curseType">�������ͣ��Ƿ��ܵ� Curse of the Labyrinth��</param>
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
    /// ���� <paramref name="result"/> �� <paramref name="remain"/> ��������ɹ�����
    /// </summary>
    /// <param name="total">�������ɷ����������Ŀ</param>
    /// <param name="remain">ʣ��������ɷ����������Ŀ</param>
    /// <param name="result">��ǰ�Ѿ��������ɷ����������Ŀ</param>
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
