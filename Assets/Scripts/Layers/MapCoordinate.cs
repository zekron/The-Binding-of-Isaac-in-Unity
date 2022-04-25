public struct MapCoordinate
{
    public int x;
    public int y;
    
    public MapCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static MapCoordinate up => new MapCoordinate(0, 1);
    public static MapCoordinate down => new MapCoordinate(0, -1);
    public static MapCoordinate left => new MapCoordinate(-1, 0);
    public static MapCoordinate right => new MapCoordinate(1, 0);
    public static MapCoordinate zero => new MapCoordinate(0, 0);

    public static MapCoordinate operator -(MapCoordinate a, MapCoordinate b)
    {
        return new MapCoordinate(a.x - b.x, a.y - b.y);
    }

    public static MapCoordinate operator +(MapCoordinate a, MapCoordinate b)
    {
        return new MapCoordinate(a.x + b.x, a.y + b.y);
    }

    public static bool operator ==(MapCoordinate a, MapCoordinate b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(MapCoordinate a, MapCoordinate b) => a.x != b.x || a.y != b.y;
}
