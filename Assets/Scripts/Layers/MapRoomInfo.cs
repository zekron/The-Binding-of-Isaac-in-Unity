using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapRoomInfo
{
    public GameCoordinate Coordinate;
    public RoomObjectType CurrentRoomType;

    private MapRoomInfo parent;
    private List<MapRoomInfo> children;

    public MapRoomInfo(GameCoordinate coordinate, MapRoomInfo parent, RoomObjectType currentRoomType = RoomObjectType.Normal)
    {
        Coordinate = coordinate;
        this.parent = parent;
        CurrentRoomType = currentRoomType;

        children = new List<MapRoomInfo>();

        if (parent != null) Depth = parent.Depth + 1;
    }

    public MapRoomInfo Parent => parent;
    public List<MapRoomInfo> Children => children;

    public int Depth;

    public override string ToString()
    {
        return string.Format("Coordinate: {0}. RoomType: {1}. Depth: {2}.", Coordinate.ToString(), CurrentRoomType, Depth);
    }
}
