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
    /// ���ڳ�ʼ����Ĳ�����ʾ
    /// </summary>
    public Sprite SpriteTip;

    public bool IsGenerateReward;
    public GameCoordinate RewardPosition;

    /// <summary>
    /// ����
    /// </summary>
    public List<TupleWithGameObjectCoordinate> monsterList = new List<TupleWithGameObjectCoordinate>();
    /// <summary>
    /// �ϰ���
    /// </summary>
    public List<TupleWithGameObjectCoordinate> obstacleList = new List<TupleWithGameObjectCoordinate>();
    /// <summary>
    /// ����
    /// </summary>
    public List<TupleWithGameObjectCoordinate> propList = new List<TupleWithGameObjectCoordinate>();
}
