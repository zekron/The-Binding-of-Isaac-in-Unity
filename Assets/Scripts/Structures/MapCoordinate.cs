using System;
using System.Collections.Generic;

[Serializable]
public struct MapCoordinate : IEquatable<MapCoordinate>, IComparer<MapCoordinate>
{
    public int x;
    public int y;

    public MapCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public MapCoordinate(MapCoordinate coordinate)
    {
        x = coordinate.x;
        y = coordinate.y;
    }

    public enum MoveDirection { Up, Down, Left, Right }
    public static MapCoordinate up => new MapCoordinate(0, 1);
    public static MapCoordinate down => new MapCoordinate(0, -1);
    public static MapCoordinate left => new MapCoordinate(-1, 0);
    public static MapCoordinate right => new MapCoordinate(1, 0);
    public static MapCoordinate one => new MapCoordinate(1, 1);
    public static MapCoordinate zero => new MapCoordinate(0, 0);

    public static readonly MapCoordinate RoomOffsetPoint = new MapCoordinate(5, 5);

    public static MapCoordinate operator -(MapCoordinate a) => new MapCoordinate(-a.x, -a.y);
    public static MapCoordinate operator -(MapCoordinate a, MapCoordinate b) => new MapCoordinate(a.x - b.x, a.y - b.y);
    public static MapCoordinate operator +(MapCoordinate a, MapCoordinate b) => new MapCoordinate(a.x + b.x, a.y + b.y);
    public static MapCoordinate operator *(MapCoordinate a, int b) => new MapCoordinate(a.x * b, a.y * b);
    public static bool operator ==(MapCoordinate a, MapCoordinate b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(MapCoordinate a, MapCoordinate b) => a.x != b.x || a.y != b.y;

    public static MapCoordinate GetMoveDirectionPoint(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                return up;
            case MoveDirection.Down:
                return down;
            case MoveDirection.Left:
                return left;
            case MoveDirection.Right:
                return right;
            default:
                return zero;
        }
    }

    public static MoveDirection[] directionArray = new MoveDirection[]
     {
        MoveDirection.Up,
        MoveDirection.Down,
        MoveDirection.Left,
        MoveDirection.Right
     };

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }

    public bool Equals(MapCoordinate other)
    {
        return x == other.x && y == other.y;
    }

    public int Compare(MapCoordinate x, MapCoordinate y)
    {
        if (x.x > y.x) return 1;
        else if (x.x == y.x)
            if (x.y > y.y)
                return 1;
            else if (x.y == y.y)
                return 0;
            else
                return -1;
        else return -1;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
