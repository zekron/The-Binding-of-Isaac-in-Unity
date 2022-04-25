using System.Collections.Generic;
using UnityEngine;

public class MapRoomInfo
{
    public MapCoordinate Coordinate;
    public RoomType CurrentRoomType;

    private Vector2 coorindateVector;
    private MapRoomInfo parent;
    private List<MapRoomInfo> children;

    public MapRoomInfo(MapCoordinate coordinate, MapRoomInfo parent, RoomType currentRoomType = RoomType.Normal)
    {
        Coordinate = coordinate;
        this.parent = parent;
        CurrentRoomType = currentRoomType;

        coorindateVector = new Vector2(Coordinate.x, Coordinate.y);
        children = new List<MapRoomInfo>();
    }

    public Vector2 CoordinateVector => coorindateVector;
    public MapRoomInfo Parent => parent;
    public List<MapRoomInfo> Children => children;
}
