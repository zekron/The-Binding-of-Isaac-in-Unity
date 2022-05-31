using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField] private FloorCurseType currentCurse = FloorCurseType.None;
	[SerializeField] private int currentLevel = 1;
	[SerializeField] private GameObject roomPrefab;
	[SerializeField] private MapCoordinateEventChannelSO onCreateRoomEvent;
	[SerializeField] private MapCoordinateStatusEventChannelSO onEnterRoomEvent;
	[SerializeField] private DoorPositionEventChannelSO onEnterDoorEvent;

	private Room[,] roomArray = new Room[GameCoordinate.RoomOffsetPoint.x << 1, GameCoordinate.RoomOffsetPoint.y << 1];
	private Room currentRoom;

	private void OnEnable()
	{
		onEnterDoorEvent.OnEventRaised += PrepareEnterRoom;
	}
	private void OnDisable()
	{
		onEnterDoorEvent.OnEventRaised -= PrepareEnterRoom;
	}

	// Start is called before the first frame update
	void Start()
	{
		CreateRooms(MapGenerator.CreateMap(ChapterType.Basement, currentLevel, currentCurse, isHardMode: false));

		//EnterRoom(DoorPosition.Up, MapCoordinate.RoomOffsetPoint);
		onEnterRoomEvent.RaiseEvent(GameCoordinate.zero, MiniMapIconStatus.Current);
		currentRoom = roomArray[GameCoordinate.RoomOffsetPoint.x, GameCoordinate.RoomOffsetPoint.y];
		currentRoom.EnterRoom(DoorPosition.Up);
		//CreateRooms(Random.Range(1, 5));
	}

	private void CreateRooms(MapRoomInfo rootRoomInfo)
	{
		Queue<MapRoomInfo> queue = new Queue<MapRoomInfo>();
		queue.Enqueue(rootRoomInfo);

		while (queue.Count > 0)
		{
			var info = queue.Dequeue();

			currentRoom = CreateRoom(info);
			roomArray[currentRoom.RoomInfo.Coordinate.x, currentRoom.RoomInfo.Coordinate.y] = currentRoom;
			CreateDoor(currentRoom);

			foreach (var child in info.Children)
			{
				queue.Enqueue(child);
			}
		}
		//for (; map.Count > 0;)
		//{
		//    currentRoom = CreateRoom(map.Dequeue());
		//    roomArray[currentRoom.RoomInfo.Coordinate.x, currentRoom.RoomInfo.Coordinate.y] = currentRoom;

		//    CreateDoor(currentRoom);
		//    lastRoom = currentRoom;
		//}
	}

	private void PrepareEnterRoom(DoorPosition doorPosition)
	{
		//if (no transport)
		EnterRoom(doorPosition, doorPosition.ToMapCoordinate() + currentRoom.RoomInfo.Coordinate);
	}

	private void EnterRoom(DoorPosition doorPosition, GameCoordinate coordinate)
	{
		//MiniMap
		onEnterRoomEvent.RaiseEvent(GameCoordinate.zero,
									currentRoom.isCleared ? MiniMapIconStatus.Explored : MiniMapIconStatus.Unexplored);
		onEnterRoomEvent.RaiseEvent(doorPosition.ToMapCoordinate(),
									MiniMapIconStatus.Current);

		currentRoom = roomArray[coordinate.x, coordinate.y];
		currentRoom.EnterRoom(doorPosition);
	}

	private Room CreateRoom(MapRoomInfo roomInfo)
	{
		var coordinate = roomInfo.Coordinate - GameCoordinate.RoomOffsetPoint;
		var result = ObjectPoolManager.Release(roomPrefab,
												new Vector3(coordinate.x * StaticData.RoomWidth,
															coordinate.y * StaticData.RoomHeight).ToWorldPosition(transform),
											   Quaternion.identity,
											   transform).GetComponent<Room>();
		result.RoomInfo = roomInfo;
		onCreateRoomEvent.RaiseEvent(roomInfo.Coordinate);

		return result;
	}

	/// <summary>
	/// 检查 <paramref name="currentRoom"></paramref> 四个方向是否有房间，有则创建门
	/// </summary>
	/// <param name="currentRoom"></param>
	/// <param name="lastRoom"></param>
	private void CreateDoor(Room currentRoom)
	{
		if (currentRoom == null || currentRoom.RoomInfo.Parent == null) return;

		MapRoomInfo parentInfo = currentRoom.RoomInfo.Parent;
		var coordinate = currentRoom.RoomInfo.Coordinate - parentInfo.Coordinate;

		roomArray[parentInfo.Coordinate.x,
				  parentInfo.Coordinate.y].CreateDoor(coordinate, currentRoom.RoomInfo.CurrentRoomType);
		currentRoom.CreateDoor(-coordinate, currentRoom.RoomInfo.CurrentRoomType);
	}
}
