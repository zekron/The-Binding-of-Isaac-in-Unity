using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RoomEditorWindow : EditorWindow
{
    RoomLayoutSO roomLayout;
    string newFileName = "�����ļ���";

    bool isExpandCreateButton = false;
    bool isExpandLoadButton = false;

    int toolBarNum;
    Vector2 scrollViewVector2;

    Vector2 centerCoordinate = new Vector2(13, 7);

    string[] monsterPrefabPath = new[]
    {
        "Assets/AssetsPackge/Prefabs/Monster/Minions/Dip.prefab",
        "Assets/AssetsPackge/Prefabs/Monster/Elite",
        "Assets/AssetsPackge/Prefabs/Monster/Boss/The Duke Of Flies.prefab",
    };
    string[] obstaclesPrefabPath = new[]
    {
        "Assets/AssetsPackge/Prefabs/Obstacles/Rock/Rock.prefab",
        "Assets/AssetsPackge/Prefabs/Obstacles/Spikes/Spikes.prefab",
        "Assets/AssetsPackge/Prefabs/Obstacles/FirePlace/FirePlace.prefab",
        "Assets/AssetsPackge/Prefabs/Obstacles/Poop/Poop.prefab",
    };
    string[] propPrefabPath = new[]
    {
        "Assets/AssetsPackge/Prefabs/Prop/Pickup/Chest/BrownChest.prefab",
        "Assets/AssetsPackge/Prefabs/Prop/Pickup/RandomPickup/RandomCoin.prefab",
        "Assets/AssetsPackge/Prefabs/Prop/Item/RandomItem/TreasureRoom Item.prefab",
        "Assets/AssetsPackge/Prefabs/Prop/Goods/ItemGoods.prefab",
    };

    //���ڻ���ذ徫�����ͼ����Ϊ���Ƶذ���Ҫ��д���ļ�����������
    private Sprite floorSprite;
    private Texture2D floorTexture;
    //����û�еذ�ʱ���ƿհ�����
    private Texture2D emptyTexture;

    private Sprite rewardSprite;
    private Sprite auxiliaryLineSprite;
    private bool IsDrawRewardPosition;
    private bool isDrawAuxiliaryLine;
    string[] editorDefaultResourcesAssetPath = new[]
    {
        "Assets/Editor Default Resources/Reward Sprite.png",
        "Assets/Editor Default Resources/Auxiliary Line.png",
    };

    //[MenuItem("Custom Menu/Window/Room Editor Window", false, 128)]
    private static void ShowWindow()
    {
        EditorWindow window = GetWindow<RoomEditorWindow>("Room Editor", true);
        window.minSize = window.maxSize = new Vector2(800, 800);
    }

    private void OnEnable()
    {
        rewardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(editorDefaultResourcesAssetPath[0]);
        auxiliaryLineSprite = AssetDatabase.LoadAssetAtPath<Sprite>(editorDefaultResourcesAssetPath[1]);
        emptyTexture = new Texture2D(StaticData.RoomWidthPixels / 2, StaticData.RoomHeightPixels / 2);
    }

    private void OnGUI()
    {
        DrawFileOperationArea();

        GUILayout.BeginVertical("Box");
        DrawFileSelectionArea();

        if (roomLayout != null) { DrawEditArea(); }
        GUILayout.EndVertical();

        if (roomLayout != null) { DrawPreviewArea(); }
        GUILayout.Box(emptyTexture);
    }

    private void DrawFileOperationArea()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("���ļ�"))
        {
            isExpandLoadButton = !isExpandLoadButton; isExpandCreateButton = false;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("�����ļ�"))
        {
            isExpandCreateButton = !isExpandCreateButton; isExpandLoadButton = false;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("�����޸�"))
        {
            if (roomLayout != null)
            {
                EditorUtility.SetDirty(roomLayout);
                AssetDatabase.SaveAssets();
            }
        }
        GUILayout.EndHorizontal();

        if (isExpandLoadButton)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("��ͨ")) { roomLayout = SelectObject(ResourcesMgr.GetNormalRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("BOSS")) { roomLayout = SelectObject(ResourcesMgr.GetBossRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("����")) { roomLayout = SelectObject(ResourcesMgr.GetTreasureRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("�̵�")) { roomLayout = SelectObject(ResourcesMgr.GetShopRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("����")) { roomLayout = SelectObject(ResourcesMgr.GetTestRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            GUILayout.EndHorizontal();
        }
        if (isExpandCreateButton)
        {
            GUILayout.BeginVertical();
            newFileName = EditorGUILayout.TextField(newFileName);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("��ͨ")) { CreateRoomLayoutFile(ResourcesMgr.GetNormalRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("BOSS")) { CreateRoomLayoutFile(ResourcesMgr.GetBossRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("����")) { CreateRoomLayoutFile(ResourcesMgr.GetTreasureRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("�̵�")) { CreateRoomLayoutFile(ResourcesMgr.GetShopRoomPath()); isExpandCreateButton = false; }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void DrawFileSelectionArea()
    {
        GUILayout.BeginHorizontal("box");
        RoomLayoutSO temp = roomLayout;
        roomLayout = (RoomLayoutSO)EditorGUILayout.ObjectField("�ļ�", roomLayout, typeof(RoomLayoutSO), false);
        GUILayout.Space(50);
        isDrawAuxiliaryLine = GUILayout.Toggle(isDrawAuxiliaryLine, "������");
        //���roomLayout�ļ��Ƿ��滻
        if (temp != roomLayout) { ResetEditInstallWhenChange(); }
        GUILayout.EndHorizontal();
    }

    private void DrawEditArea()
    {
        GUILayout.Space(5);
        toolBarNum = GUILayout.Toolbar(toolBarNum, new[] { "��Ҫ", "�ϰ����б�", "�����б�", "�����б�" });

        GUILayout.BeginVertical("box");
        GUILayout.Space(5);
        switch (toolBarNum)
        {
            case 0:
                EditMain();
                break;
            case 1:
                EditObstacles();
                break;
            case 2:
                EditMonster();
                break;
            case 3:
                EditorProp();
                break;
            default:
                break;
        }
        GUILayout.EndVertical();
    }
    private void EditMain()
    {
        GUILayout.BeginHorizontal();
        roomLayout.SpriteFloor = (Sprite)EditorGUILayout.ObjectField("�ذ�", roomLayout.SpriteFloor, typeof(Sprite), false);
        roomLayout.SpriteTip = (Sprite)EditorGUILayout.ObjectField("Tip", roomLayout.SpriteTip, typeof(Sprite), false);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        roomLayout.IsGenerateReward = GUILayout.Toggle(roomLayout.IsGenerateReward, "���ɽ���Ʒ");
        IsDrawRewardPosition = GUILayout.Toggle(IsDrawRewardPosition, "����") && roomLayout.IsGenerateReward;
        GUILayout.Space(10);
        GUILayout.Label("X");
        int x = EditorGUILayout.IntSlider((int)roomLayout.RewardPosition.x, 1, 25);
        GUILayout.Space(10);
        GUILayout.Label("Y");
        int y = EditorGUILayout.IntSlider((int)roomLayout.RewardPosition.y, 1, 13);
        roomLayout.RewardPosition = new Vector2(x, y);
        GUILayout.EndHorizontal();
    }
    private void EditObstacles()
    {
        //����ѡ���ļ���
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("��ʯ")) { SelectObject(obstaclesPrefabPath[0]); }
        if (GUILayout.Button("���")) { SelectObject(obstaclesPrefabPath[1]); }
        if (GUILayout.Button("���")) { SelectObject(obstaclesPrefabPath[2]); }
        if (GUILayout.Button("ʺ��")) { SelectObject(obstaclesPrefabPath[3]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        //�����ļ����ƹ���͹�������ı༭����
        EditObjectList(roomLayout.obstacleList);
    }
    private void EditMonster()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("��ͨ")) { SelectObject(monsterPrefabPath[0]); }
        if (GUILayout.Button("��Ӣ")) { SelectObject(monsterPrefabPath[1]); }
        if (GUILayout.Button("Boss")) { SelectObject(monsterPrefabPath[2]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditObjectList(roomLayout.monsterList);
    }
    private void EditorProp()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����")) { SelectObject(propPrefabPath[0]); }
        if (GUILayout.Button("���ʰȡ��")) { SelectObject(propPrefabPath[1]); }
        if (GUILayout.Button("�������")) { SelectObject(propPrefabPath[2]); }
        if (GUILayout.Button("�����Ʒ")) { SelectObject(propPrefabPath[3]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditObjectList(roomLayout.propList);
    }

    private void DrawPreviewArea()
    {
        //���Ƶذ壬���ذ徫�鲻һ����Ϊ��ʱ�����µĵذ���ͼ��Ϊ��ʱ�����հ���ͼ
        if (roomLayout.SpriteFloor != null)
        {
            if (floorSprite == null || floorSprite != roomLayout.SpriteFloor)
            {
                floorSprite = roomLayout.SpriteFloor;
                floorTexture = GetFloorTexture(roomLayout.SpriteFloor);
            }
            GUILayout.Box(floorTexture);
        }
        else { GUILayout.Box(emptyTexture); }

        //���Ʒ����ڵ�����
        if (Event.current.type == EventType.Repaint)
        {
            //����tip
            if (roomLayout.SpriteTip != null) { DrawSprite(roomLayout.SpriteTip, centerCoordinate); }
            //�����ϰ���
            DrawObjectList(roomLayout.obstacleList);
            //���ƹ���
            DrawObjectList(roomLayout.monsterList);
            //���Ƶ���
            DrawObjectList(roomLayout.propList);
            //���ƽ���λ��
            if (IsDrawRewardPosition)
            {
                Vector2 coordinate = roomLayout.RewardPosition;
                DrawSprite(rewardSprite, coordinate);
            }
            //���Ƹ�����
            if (isDrawAuxiliaryLine)
            {
                DrawSprite(auxiliaryLineSprite, centerCoordinate);
            }
        }
    }

    /// <summary>
    /// ����·��ѡ��Asset�µ��ļ����ļ���
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private UnityEngine.Object SelectObject(string path)
    {
        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);
        if (obj == null) { Debug.Log("�ļ������ڣ� " + path); return null; }
        Selection.activeObject = obj;
        return obj;
    }

    /// <summary>
    /// �����µĲ����ļ���ѡ����ļ�
    /// </summary>
    /// <param name="path"></param>
    private void CreateRoomLayoutFile(string path)
    {
        RoomLayoutSO go = ScriptableObject.CreateInstance<RoomLayoutSO>();
        string newPath = Path.Combine(path, newFileName + ".asset");
        AssetDatabase.CreateAsset(go, newPath);
        roomLayout = SelectObject(newPath) as RoomLayoutSO;
        roomLayout.IsGenerateReward = true;
        roomLayout.RewardPosition = centerCoordinate;
        ResetEditInstallWhenCreate();
    }

    /// <summary>
    /// ����Ԥ�����������Ʊ༭���У��и߶�����(170)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabs"></param>
    /// <param name="coordinates"></param>
    /// <param name="remove"></param>
    /// <param name="add"></param>
    private void EditObjectList(List<TupleWithGameObjectVector2> prefabs)
    {
        scrollViewVector2 = GUILayout.BeginScrollView(scrollViewVector2, GUILayout.Height(160));
        for (int i = 0; i < prefabs.Count; i++)
        {
            GUILayout.BeginHorizontal();
            prefabs[i].value1 = (GameObject)EditorGUILayout.ObjectField(prefabs[i].value1, typeof(GameObject), false);
            GUILayout.Space(30);
            GUILayout.Label("X");
            int x = EditorGUILayout.IntSlider((int)prefabs[i].value2.x, 1, 25);
            GUILayout.Space(10);
            GUILayout.Label("Y");
            int y = EditorGUILayout.IntSlider((int)prefabs[i].value2.y, 1, 13);
            prefabs[i].value2 = new Vector2(x, y);
            GUILayout.Space(10);
            if (GUILayout.Button("�Ƴ�")) { prefabs.RemoveAt(i); }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("���", GUILayout.MaxWidth(75))) { prefabs.Add(new TupleWithGameObjectVector2(null, centerCoordinate)); }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// ����RoomLayoutʱ�����������
    /// </summary>
    private void ResetEditInstallWhenChange()
    {
        toolBarNum = 0;
        isDrawAuxiliaryLine = false;
        IsDrawRewardPosition = true;
    }

    /// <summary>
    /// �½�RoomLayoutʱ�����������
    /// </summary>
    private void ResetEditInstallWhenCreate()
    {
        toolBarNum = 0;
        isDrawAuxiliaryLine = true;
        IsDrawRewardPosition = true;
    }

    /// <summary>
    /// ����Ԥ������������Ԥ��ͼ��
    /// </summary>
    /// <param name="prefabs"></param>
    /// <param name="coordinates"></param>
    private void DrawObjectList(List<TupleWithGameObjectVector2> prefabs)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].value1 != null)
            {
                Sprite sprite = prefabs[i].value1.GetComponent<SpriteRenderer>().sprite;
                Vector2 coordinate = prefabs[i].value2;
                DrawSprite(sprite, coordinate);
            }
        }
    }

    /// <summary>
    /// ���Ƶ������飻���ȡ�������ͼ���ٸ��ݾ����Rect����ͼ���ȡ�������
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="coordinate"></param>
    private void DrawSprite(Sprite sprite, Vector2 coordinate)
    {
        if (sprite == null) { Debug.Log("ͼƬΪ��"); return; }

        //�������λ��
        //���=��ǰBox��Rect(���Ͻ�,��Box�����)+��Ե����
        Vector2 outset = GUILayoutUtility.GetLastRect().position + new Vector2(3, 3);
        //���ĵ�=���+�ذ���ͼ��С/2
        Vector2 center = outset + new Vector2(emptyTexture.width / 2, emptyTexture.height / 2);
        //����λ��=���ĵ�+ƫ��(����*���ش�С)-�����һ���С(����������λ�����Ͻǣ���ȥ�����С��һ��ʹ����ʾʱ����������ĵ���ǰ������λ��)
        int UnitPixels = (int)(StaticData.RoomUnitSize * 100 / 2);
        Vector2 pos = center + new Vector2(-(StaticData.RoomHorizontalUnit - coordinate.x), -(StaticData.RoomVerticalUnit - coordinate.y)) * UnitPixels - new Vector2(sprite.rect.width / 2, sprite.rect.height / 2);

        //���û��Ƶ�λ�úʹ�С
        Rect displayArea = sprite.rect;
        float spriteW = displayArea.width;
        float spriteH = displayArea.height;
        Rect newRect = new Rect(pos, new Vector2(spriteW, spriteH));

        //��Ϊ4��������СҪ��Ϊ0-1֮�䣬���Գ���ԭ��ͼ���õ�����
        var tex = sprite.texture;
        displayArea.xMin /= tex.width;
        displayArea.xMax /= tex.width;
        displayArea.yMin /= tex.height;
        displayArea.yMax /= tex.height;

        //���������ֱ�Ϊ:���Ƶ�λ�úʹ�С,ԭ��ͼ��ԭ��ͼ��ȡ������
        GUI.DrawTextureWithTexCoords(newRect, tex, displayArea);
    }
    /// <summary>
    /// ����������3����ת��ͼ������Ϊһ����ͼ����
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    private Texture2D GetFloorTexture(Sprite sprite)
    {
        //������ľ�������Ϊ��ͼ��ע�⣺��Ҫ����ԭ��ͼ���� �߼����ɶ�д
        var rect = sprite.rect;
        int width = (int)rect.width;
        int height = (int)rect.height;
        var texture = new Texture2D(width, height);
        var data = sprite.texture.GetPixels((int)rect.x, (int)rect.y, width, height);
        texture.SetPixels(data);
        texture.Apply(true);

        //����3�ŷ�ת��ͼ
        var upRightTexture = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            upRightTexture.SetPixels(i, 0, 1, height, texture.GetPixels(width - i - 1, 0, 1, height));
        }

        var downLeftTexture = new Texture2D(width, height);
        for (int i = 0; i < height; i++)
        {
            downLeftTexture.SetPixels(0, i, width, 1, texture.GetPixels(0, height - i - 1, width, 1));
        }

        var downRightTexture = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            downRightTexture.SetPixels(i, 0, 1, height, downLeftTexture.GetPixels(width - i - 1, 0, 1, height));
        }

        //����4��1����ͼ������
        var newTexture = new Texture2D(width * 2, height * 2);
        newTexture.SetPixels(0, height, width, height, texture.GetPixels());
        newTexture.SetPixels(width, height, width, height, upRightTexture.GetPixels());
        newTexture.SetPixels(0, 0, width, height, downLeftTexture.GetPixels());
        newTexture.SetPixels(width, 0, width, height, downRightTexture.GetPixels());
        newTexture.Apply();
        return newTexture;
    }
}
