using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// </summary>
public class Room : MonoBehaviour
{
    [Header("��������")]

    [HideInInspector]
    public RoomType roomType;//��������

    [HideInInspector]
    public Vector2 coordinate;//����

    private RoomLayoutSO roomLayout;//�����ļ�

    public bool isArrived = false;//�Ƿ��ѵ���
    public bool isCleared = false;//�Ƿ��������

    //�����ߣ�����ֵ/100
    public static float RoomWidth { get { return 15.06f; } }
    public static float RoomHeight { get { return 9.44f; } }
    public static int RoomWidthPixels { get { return 1506; } }
    public static int RoomHeightPixels { get { return 944; } }

    //��λ�����ʹ�С
    public static int HorizontalUnit { get { return 13; } }
    public static int VerticalUnit { get { return 7; } }
    public static float UnitSize { get { return 0.28f; } }
}
