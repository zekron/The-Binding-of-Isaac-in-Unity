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
    public Vector2 RewardPosition;

    /// <summary>
    /// ����
    /// </summary>
    public List<TupleWithGameObjectVector2> monsterList = new List<TupleWithGameObjectVector2>();
    /// <summary>
    /// �ϰ���
    /// </summary>
    public List<TupleWithGameObjectVector2> obstacleList = new List<TupleWithGameObjectVector2>();
    /// <summary>
    /// ����
    /// </summary>
    public List<TupleWithGameObjectVector2> propList = new List<TupleWithGameObjectVector2>();
}
