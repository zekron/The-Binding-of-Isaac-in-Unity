using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New RoomLayout",menuName ="Scriptable Object/Room Layout")]
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
    public Vector2 RewardPosition;

    /// <summary>
    /// 敌人
    /// </summary>
    public List<TupleWithGameObjectVector2> monsterList = new List<TupleWithGameObjectVector2>();
    /// <summary>
    /// 障碍物
    /// </summary>
    public List<TupleWithGameObjectVector2> obstacleList = new List<TupleWithGameObjectVector2>();
    /// <summary>
    /// 道具
    /// </summary>
    public List<TupleWithGameObjectVector2> propList = new List<TupleWithGameObjectVector2>();
}
