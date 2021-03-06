using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameLogicUtility
{

    public static Vector3 LocalPointToWorldPoint(Transform parent, Vector3 localPoint)
    {
        return localPoint + parent.position;
    }

    /// <summary>
    /// ϴ??
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        int num = list.Count;
        int ran;
        while (num > 0)
        {
            ran = Random.Range(0, num);
            T temp = list[ran];
            list[ran] = list[num - 1];
            list[num - 1] = temp;
            num--;
        }
        return list;
    }

    public static string ToString<T>(this IList<T> list)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in list)
        {
            sb.AppendLine(item.ToString());
        }
        return sb.ToString();
    }

    public static Queue<T> Reverse<T>(this Queue<T> queue)
    {
        var stack = new Stack<T>(queue.Count);
        for (; queue.Count > 0;)
        {
            stack.Push(queue.Dequeue());
        }
        for (; stack.Count > 0;)
        {
            queue.Enqueue(stack.Pop());
        }
        return queue;
    }

    public static Texture2D GetEmptyTexture(int width, int height)
    {
        var result = new Texture2D(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result.SetPixel(x, y, Color.clear);
            }
        }
        result.Apply();
        return result;
    }

    public static DoorPosition ToDoorPosition(this MapCoordinate mapCoordinate)
    {
        if (mapCoordinate == MapCoordinate.up) return DoorPosition.Up;
        else if (mapCoordinate == MapCoordinate.right) return DoorPosition.Right;
        else if (mapCoordinate == MapCoordinate.left) return DoorPosition.Left;
        else return DoorPosition.Down;
    }
    public static MapCoordinate ToMapCoordinate(this DoorPosition doorPosition)
    {
        switch (doorPosition)
        {
            case DoorPosition.Up:
                return MapCoordinate.up;
            case DoorPosition.Down:
                return MapCoordinate.down;
            case DoorPosition.Left:
                return MapCoordinate.left;
            case DoorPosition.Right:
                return MapCoordinate.right;
            default:
                return MapCoordinate.zero;
        }
    }
    public static Vector3 ToVector3(this DoorPosition doorPosition)
    {
        switch (doorPosition)
        {
            case DoorPosition.Up:
                return Vector3.up;
            case DoorPosition.Down:
                return Vector3.down;
            case DoorPosition.Left:
                return Vector3.left;
            case DoorPosition.Right:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }
}
