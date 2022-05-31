using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间相关
/// </summary>
public class Room : MonoBehaviour
{
	[Header("房间属性")]
	public MapRoomInfo RoomInfo;

	[SerializeField] private RoomLayoutSO roomLayout;//布局文件
	[SerializeField] private TwoVector3EventChannelSO onEnterRoomEvent;

	public bool isArrived = false;//是否已到达
	public bool isCleared = false;//是否已清理过

	#region 房间布局
	[Header("房间布局")]
	[SerializeField] private SpriteRenderer[] topBottomWallSprite;
	[SerializeField] private SpriteRenderer[] leftRightWallSprite;
	[SerializeField] private SpriteRenderer floorSprite;
	[SerializeField] private Transform doorTransform;
	[SerializeField] private Transform layoutTransform;
	[SerializeField] private GameObject[] doorLocks;
	private List<Door> roomDoors;
	private int doorsCount => roomDoors.Count;
	#endregion

	#region Test
	[Header("Test Part")]
	[SerializeField] private GameObject normalDoor;
	[SerializeField] private GameObject bossDoor;
	[SerializeField] private GameObject treasureDoor;
	[SerializeField] private GameObject shopDoor;
	[SerializeField] private GameObject secretDoor;
	[SerializeField] private GameObject arcadeDoor;
	[SerializeField] private GameObject bossChallengeDoor;
	[SerializeField] private GameObject challengeDoor;
	[SerializeField] private GameObject curseDoor;
	#endregion

	private void OnEnable()
	{
		RoomLayoutInitialize();
		if (roomDoors == null)
		{
			roomDoors = new List<Door>(4);
		}
	}

	private void OnDisable()
	{

	}

	private void Start()
	{

	}

	private void RoomLayoutInitialize()
	{
		floorSprite.sprite = roomLayout.SpriteFloor;
		for (int i = 0; i < topBottomWallSprite.Length; i++)
		{
			topBottomWallSprite[i].sprite = roomLayout.SpriteTop;
			leftRightWallSprite[i].sprite = roomLayout.SpriteLeft;
		}

		for (int i = 0; i < roomLayout.obstacleList.Count; i++)
		{
			var obstacle = ObjectPoolManager.Release(
					roomLayout.obstacleList[i].value1,
					roomLayout.obstacleList[i].value2.ToRoomPosition().ToWorldPosition(transform),
					Quaternion.identity,
					layoutTransform).GetComponent<IObjectInRoom>();

			obstacle.Coordinate = roomLayout.obstacleList[i].value2;
			obstacle.ChangeRendererOrder();
		}
	}

	public void EnterRoom(DoorPosition doorPosition)
	{
		var playerPosition = transform.position;
		switch (doorPosition)
		{
			case DoorPosition.Up:
				playerPosition.y -= 2.84f;
				break;
			case DoorPosition.Down:
				playerPosition.y += 2.84f;
				break;
			case DoorPosition.Left:
				playerPosition.x += 5.64f;
				break;
			case DoorPosition.Right:
				playerPosition.x -= 5.64f;
				break;
			default:
				break;
		}
		onEnterRoomEvent.RaiseEvent(transform.localPosition, playerPosition);

		isCleared = true;
		if (isCleared)
		{
			foreach (var door in roomDoors)
			{
				doorLocks[(int)door.doorPosition].SetActive(false);
				door.RaiseEvent(DoorStatus.Open);
			}
		}
	}

	public void CreateDoor(GameCoordinate direction, RoomType roomType = RoomType.Normal)
	{
		var tempTransform = Door.GetDoorTransform(direction.ToDoorPosition());

		roomDoors.Add(ObjectPoolManager.Release(GetDoorPrefabWithRoomType(roomType),
												tempTransform.localPosition.ToWorldPosition(transform),
												tempTransform.rotation,
												doorTransform).GetComponent<Door>());
		roomDoors[doorsCount - 1].doorPosition = direction.ToDoorPosition();
		//test
		//roomDoors[doorsCount - 1].RaiseEvent(DoorStatus.Open);
		//onDoorStatusChanged.RaiseEvent(DoorStatus.Open);
	}

	private GameObject GetDoorPrefabWithRoomType(RoomType type)
	{
		switch (type)
		{
			case RoomType.Starting:
			case RoomType.Normal:
			case RoomType.MiniBoss:
			case RoomType.Error:
			case RoomType.Library:
			case RoomType.Sacrifice:
				return normalDoor;
			case RoomType.Boss:
				return bossDoor;
			case RoomType.Devil:
				break;
			case RoomType.Angel:
				break;
			case RoomType.Treasure:
				return treasureDoor;
			case RoomType.Shop:
				return shopDoor;
			case RoomType.Arcade:
				return arcadeDoor;
			case RoomType.Challenge:
				return challengeDoor;
			case RoomType.BossChallenge:
				return bossChallengeDoor;
			case RoomType.Curse:
				return curseDoor;
			case RoomType.Secret:
			case RoomType.SuperSecret:
				return secretDoor;
		}
		return normalDoor;
	}
}
