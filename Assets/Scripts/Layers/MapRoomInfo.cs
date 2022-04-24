using UnityEngine;

public struct MapRoomInfo
{
    public (int x, int y) Coordinate;
    public RoomType CurrentRoomType;

    public MapRoomInfo((int x, int y) coordinate, RoomType currentRoomType = RoomType.Normal)
    {
        Coordinate = coordinate;
        CurrentRoomType = currentRoomType;
    }

    public Vector2 CoordinateVector => new Vector2(Coordinate.x, Coordinate.y);
}
