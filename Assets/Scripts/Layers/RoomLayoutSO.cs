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
    /// ���ڳ�ʼ����Ĳ�����ʾ
    /// </summary>
    public Sprite SpriteTip;

    public bool IsGenerateReward;
    public GameCoordinate RewardPosition;

    /// <summary>
    /// ����
    /// </summary>
    public List<TupleWithGameObjectCoordinate> monsterList = new List<TupleWithGameObjectCoordinate>();
    //public List<TupleWithRoomObjectCoordinate> monsterList = new List<TupleWithRoomObjectCoordinate>();
    /// <summary>
    /// �ϰ���
    /// </summary>
    public List<TupleWithGameObjectCoordinate> obstacleList = new List<TupleWithGameObjectCoordinate>();
    //public List<TupleWithRoomObjectCoordinate> obstacleList = new List<TupleWithRoomObjectCoordinate>();
    /// <summary>
    /// ����
    /// </summary>
    //public List<TupleWithGameObjectCoordinate> propList = new List<TupleWithGameObjectCoordinate>();
    public List<TupleWithRandomPickupCoordinate> propList = new List<TupleWithRandomPickupCoordinate>();
}
