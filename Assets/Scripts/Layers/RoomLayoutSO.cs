using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New RoomLayout",menuName ="Scriptable Object/Room Layout/Basic Room Layout")]
public class RoomLayoutSO : ScriptableObject
{
    public Sprite SpriteFloor;
    public Sprite SpriteTop;
    public Sprite SpriteLeft;

    /// <summary>
    /// 用于初始房间的操作提示
    /// </summary>
    public Sprite SpriteTip;

    public bool IsGenerateReward;
    public GameCoordinate RewardPosition;

    /// <summary>
    /// 敌人
    /// </summary>
    public List<TupleWithGameObjectCoordinate> monsterList = new List<TupleWithGameObjectCoordinate>();
    //public List<TupleWithRoomObjectCoordinate> monsterList = new List<TupleWithRoomObjectCoordinate>();
    /// <summary>
    /// 障碍物
    /// </summary>
    public List<TupleWithGameObjectCoordinate> obstacleList = new List<TupleWithGameObjectCoordinate>();
    //public List<TupleWithRoomObjectCoordinate> obstacleList = new List<TupleWithRoomObjectCoordinate>();
    /// <summary>
    /// 道具
    /// </summary>
    //public List<TupleWithGameObjectCoordinate> propList = new List<TupleWithGameObjectCoordinate>();
    public List<TupleWithRandomPickupCoordinate> propList = new List<TupleWithRandomPickupCoordinate>();
}
