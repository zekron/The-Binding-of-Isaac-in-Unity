using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameLogicUtility
{
	public static Vector3 ToWorldPosition(this Vector3 localPosition, Transform parent)
	{
		return localPosition + parent.position;
	}

	public static Vector3 ToRoomPosition(this GameCoordinate coordinate)
	{
		return new Vector3(coordinate.x * StaticData.RoomHorizontalUnitSize, coordinate.y * StaticData.RoomHorizontalUnitSize);
	}

	/// <summary>
	/// Ï´ÅÆ
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

	public static DoorPosition ToDoorPosition(this GameCoordinate mapCoordinate)
	{
		if (mapCoordinate == GameCoordinate.up) return DoorPosition.Up;
		else if (mapCoordinate == GameCoordinate.right) return DoorPosition.Right;
		else if (mapCoordinate == GameCoordinate.left) return DoorPosition.Left;
		else return DoorPosition.Down;
	}
	public static GameCoordinate ToMapCoordinate(this DoorPosition doorPosition)
	{
		switch (doorPosition)
		{
			case DoorPosition.Up:
				return GameCoordinate.up;
			case DoorPosition.Down:
				return GameCoordinate.down;
			case DoorPosition.Left:
				return GameCoordinate.left;
			case DoorPosition.Right:
				return GameCoordinate.right;
			default:
				return GameCoordinate.zero;
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
