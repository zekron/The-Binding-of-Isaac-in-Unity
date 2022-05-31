using System;
using System.Collections.Generic;

[Serializable]
public struct GameCoordinate : IEquatable<GameCoordinate>, IComparer<GameCoordinate>
{
    public int x;
    public int y;

    public GameCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public GameCoordinate(GameCoordinate coordinate)
    {
        x = coordinate.x;
        y = coordinate.y;
    }

    public enum MoveDirection { Up, Down, Left, Right }
    public static GameCoordinate up => new GameCoordinate(0, 1);
    public static GameCoordinate down => new GameCoordinate(0, -1);
    public static GameCoordinate left => new GameCoordinate(-1, 0);
    public static GameCoordinate right => new GameCoordinate(1, 0);
    public static GameCoordinate one => new GameCoordinate(1, 1);
    public static GameCoordinate zero => new GameCoordinate(0, 0);

    public static readonly GameCoordinate RoomOffsetPoint = new GameCoordinate(5, 4);

    public static GameCoordinate operator -(GameCoordinate a) => new GameCoordinate(-a.x, -a.y);
    public static GameCoordinate operator -(GameCoordinate a, GameCoordinate b) => new GameCoordinate(a.x - b.x, a.y - b.y);
    public static GameCoordinate operator +(GameCoordinate a, GameCoordinate b) => new GameCoordinate(a.x + b.x, a.y + b.y);
    public static GameCoordinate operator *(GameCoordinate a, int b) => new GameCoordinate(a.x * b, a.y * b);
    public static bool operator ==(GameCoordinate a, GameCoordinate b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(GameCoordinate a, GameCoordinate b) => a.x != b.x || a.y != b.y;

    public static GameCoordinate GetMoveDirectionPoint(MoveDirection direction)
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

    public bool Equals(GameCoordinate other)
    {
        return x == other.x && y == other.y;
    }

    public int Compare(GameCoordinate x, GameCoordinate y)
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
