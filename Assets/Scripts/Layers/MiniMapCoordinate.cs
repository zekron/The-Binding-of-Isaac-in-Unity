using System;

public struct MiniMapCoordinate : IEquatable<MiniMapCoordinate>
{
    public int x;
    public int y;
    public MiniMapIconStatus status;

    public MiniMapCoordinate(int x, int y, MiniMapIconStatus status)
    {
        this.x = x;
        this.y = y;
        this.status = status;
    }
    public MiniMapCoordinate(MapCoordinate coordinate, MiniMapIconStatus status)
    {
        x = coordinate.x;
        y = coordinate.y;
        this.status = status;
    }
    public bool Equals(MiniMapCoordinate other)
    {
        return x == other.x && y == other.y && status == other.status;
    }
}