using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    private static List<MapCoordinate> coordinateList = new List<MapCoordinate>();
    private static List<MapRoomInfo> deadEndList = new List<MapRoomInfo>();
    private static Queue<MapRoomInfo> coordinateQueue = new Queue<MapRoomInfo>();

    public static MapRoomInfo CreateMap(ChapterType chapterType, int floorDepth, FloorCurseType curseType, bool isHardMode)
    {
        coordinateList.Clear();
        coordinateQueue.Clear();
        deadEndList.Clear();

        //���ɳ�ʼ�����ͼ
        var roomNumber = GetRoomNumberWithLevelFloorDepth(floorDepth, curseType, isHardMode);
        var deadEnds = GetMinDeadEndsWithFloorDepth(floorDepth, curseType);
        var result = new MapRoomInfo(MapCoordinate.RoomOffsetPoint, null);
        coordinateQueue.Enqueue(result);
        deadEndList.Add(result);
        coordinateList.Add(MapCoordinate.RoomOffsetPoint);
        roomNumber--;

        GenerateMainPath(result, roomNumber - deadEnds);

        coordinateQueue.Reverse();
        while (coordinateQueue.Count > 0 && deadEnds > 0)
        {
            if (InsertDeadEnd(coordinateQueue.Peek()))
            {
                deadEnds--;
                coordinateQueue.Enqueue(coordinateQueue.Dequeue());
            }
            else
            {
                coordinateQueue.Dequeue();
            }
        }


        //while (deadEndList.Count < deadEnds)

        //TODO: �����·�������꣬�������ⷿ��
        deadEndList.Sort((x, y) => y.Depth - x.Depth);

        PlaceRoom(RoomType.Boss);
        PlaceRoom(RoomType.SuperSecret);
        if (CanPlaceShopRoom(floorDepth))
        {
            PlaceRoom(RoomType.Shop);
        }
        if (CanPlaceTreasureRoom(floorDepth))
        {
            PlaceRoom(RoomType.Treasure);
        }
        TryPlacingSecret();

        return result;
    }

    private static bool CanPlaceShopRoom(int floorDepth)
    {
        return floorDepth < 7;
    }

    private static bool CanPlaceTreasureRoom(int floorDepth)
    {
        return floorDepth < 7;
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
        if (floorDepth > 3 && Random.value >= 0.5f) minDeadEnds += 1;

        return minDeadEnds;
    }

    private static void GenerateMainPath(MapRoomInfo curRoomInfo, int roomNumber)
    {
        var tempInfo = curRoomInfo;
        int cnt = 0;
        while (roomNumber - cnt > 0)
        {
            if (Random.value > 0.5f) MapCoordinate.directionArray.Shuffle();
            for (int i = 0; i < MapCoordinate.directionArray.Length; i++)
            {
                if (CanGenerate(tempInfo.Coordinate, MapCoordinate.directionArray[i]))
                {
                    tempInfo = CreateCoordinate(tempInfo, MapCoordinate.directionArray[i]);
                    coordinateQueue.Enqueue(tempInfo);
                    break;
                }
            }
            cnt++;
            if ((cnt & 7) == 0)
            {
                tempInfo = curRoomInfo;
            }
        }
        deadEndList.Add(tempInfo);
    }

    private static bool InsertDeadEnd(MapRoomInfo curRoomInfo)
    {
        bool isSuccess = false;
        for (int i = 0; i < MapCoordinate.directionArray.Length; i++)
        {
            if (CanGenerate(curRoomInfo.Coordinate, MapCoordinate.directionArray[i]))
            {
                deadEndList.Add(CreateCoordinate(curRoomInfo, MapCoordinate.directionArray[i]));
                isSuccess = true;
                break;
            }
        }

        if (isSuccess && !IsDeadEndCoordinate(curRoomInfo) && deadEndList.Contains(curRoomInfo))
        {
            deadEndList.Remove(curRoomInfo);
        }

        return isSuccess;
    }

    private static MapRoomInfo CreateCoordinate(MapRoomInfo curRoomInfo, MapCoordinate.MoveDirection direction)
    {
        var newCoordinate = curRoomInfo.Coordinate + MapCoordinate.GetMoveDirectionPoint(direction);
        coordinateList.Add(newCoordinate);
        var newPoint = new MapRoomInfo(newCoordinate, curRoomInfo);
        curRoomInfo.Children.Add(newPoint);

        return newPoint;
    }

    private static void PlaceRoom(RoomType roomType)
    {
        if (deadEndList.Count <= 0) return;

        MapRoomInfo roomInfo = deadEndList[0];
        switch (roomType)
        {
            case RoomType.Boss:
                deadEndList[0].CurrentRoomType = RoomType.Boss;
                deadEndList.RemoveAt(0);
                break;
            case RoomType.SuperSecret:
                deadEndList[0].CurrentRoomType = RoomType.SuperSecret;
                deadEndList.RemoveAt(0);
                break;
            case RoomType.Shop:
                deadEndList[0].CurrentRoomType = RoomType.Shop;
                deadEndList.RemoveAt(0);
                break;
            case RoomType.Treasure:
                deadEndList[0].CurrentRoomType = RoomType.Treasure;
                deadEndList.RemoveAt(0);
                break;
            case RoomType.Sacrifice:
                break;
            case RoomType.Library:
                break;
            case RoomType.Curse:
                break;
            case RoomType.MiniBoss:
                break;
            case RoomType.Challenge:
                break;
            case RoomType.BossChallenge:
                break;
            case RoomType.Arcade:
                break;
            case RoomType.Secret:
                break;
            case RoomType.Starting:
                break;
            default:
                break;
        }
    }

    private static void TryPlacingSecret()
    {
        //throw new System.NotImplementedException();
    }

    private static bool CanGenerate(MapCoordinate point, MapCoordinate.MoveDirection direction)
    {
        MapCoordinate coordinate = point + MapCoordinate.GetMoveDirectionPoint(direction);
        bool result = !IsOutOfBound(coordinate);
        if (result) result = !HasBeenOccupied(coordinate);
        if (result) result = !HasAnyNeighbours(coordinate);
        return result;
    }

    private static bool IsOutOfBound(MapCoordinate coordinate)
    {
        return coordinate.x >= MapCoordinate.RoomOffsetPoint.x * 2
            || coordinate.x < 0
            || coordinate.y >= MapCoordinate.RoomOffsetPoint.y * 2
            || coordinate.y < 0;
    }

    private static bool HasBeenOccupied(MapCoordinate coordinate)
    {
        return coordinateList.Contains(coordinate);
    }

    /// <summary>
    /// <paramref name="coordinate"/> �Ƿ��� <paramref name="neighbourNum"/> ���ھ�
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="neighbourNum"></param>
    /// <returns></returns>
    private static bool HasAnyNeighbours(MapCoordinate coordinate, int neighbourNum = 2)
    {
        bool result = false;
        int count = 0;
        for (int i = 0; i < MapCoordinate.directionArray.Length; i++)
        {
            if (HasBeenOccupied(coordinate + MapCoordinate.GetMoveDirectionPoint(MapCoordinate.directionArray[i]))) count++;

            if (count >= neighbourNum) return true;
        }
        return result;
    }

    private static bool IsDeadEndCoordinate(MapRoomInfo roomInfo)
    {
        return (roomInfo.Parent == null && roomInfo.Children.Count == 1)
            || (roomInfo.Parent != null && roomInfo.Children.Count == 0);
    }

    /// <summary>
    /// ���� <paramref name="result"/> �� <paramref name="remain"/> ��������ɹ�����
    /// </summary>
    /// <param name="total">�������ɷ����������Ŀ</param>
    /// <param name="remain">ʣ��������ɷ����������Ŀ</param>
    /// <param name="result">��ǰ�Ѿ��������ɷ����������Ŀ</param>
    /// <returns></returns>
    private static float Percent(int total, int remain)
    {
        //TODO
        return (float)remain / total;
    }

    //private static MapCoordinate GetMoveDirectionPoint(MoveDirection direction)
    //{
    //    switch (direction)
    //    {
    //        case MoveDirection.Up:
    //            return MapCoordinate.up;
    //        case MoveDirection.Down:
    //            return MapCoordinate.down;
    //        case MoveDirection.Left:
    //            return MapCoordinate.left;
    //        case MoveDirection.Right:
    //            return MapCoordinate.right;
    //        default:
    //            return MapCoordinate.zero;
    //    }
    //}

    //static MoveDirection[] directionArray = new MoveDirection[]
    // {
    //    MoveDirection.Up,
    //    MoveDirection.Down,
    //    MoveDirection.Left,
    //    MoveDirection.Right
    // };
    //enum MoveDirection { Up, Down, Left, Right }
}
