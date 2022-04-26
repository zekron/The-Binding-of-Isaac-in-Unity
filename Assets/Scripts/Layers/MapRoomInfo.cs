using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapRoomInfo
{
    public MapCoordinate Coordinate;
    public RoomType CurrentRoomType;

    private MapRoomInfo parent;
    private List<MapRoomInfo> children;

    public MapRoomInfo(MapCoordinate coordinate, MapRoomInfo parent, RoomType currentRoomType = RoomType.Normal)
    {
        Coordinate = coordinate;
        this.parent = parent;
        CurrentRoomType = currentRoomType;

        children = new List<MapRoomInfo>();
    }

    public MapRoomInfo Parent => parent;
    public List<MapRoomInfo> Children => children;

    public int Depth
    {
        get
        {
            int result = 0; var info = this;
            while (info.parent != null)
            {
                result++;
                info = info.parent;
            }
            return result;
        }
    }

    public override string ToString()
    {
        return string.Format("Coordinate: {0}. RoomType: {1}. Depth: {2}.", Coordinate.ToString(), CurrentRoomType, Depth);
    }
}
