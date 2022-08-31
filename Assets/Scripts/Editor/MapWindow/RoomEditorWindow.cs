using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class RoomEditorWindow : EditorWindow
{
    private const string EXTENSION = ".asset";
    RoomLayoutSO roomLayout;
    RoomEditorWindow roomEditorWindow;

    string newFileName = "�����ļ���";

    bool isExpandCreateButton = false;
    bool isExpandLoadButton = false;

    int toolBarNum;
    Vector2 scrollViewVector2;

    GameCoordinate centerCoordinate = new GameCoordinate(13, 7);

    string[] monsterPrefabPath = new[]
    {
        "Assets/AssetsPackge/Prefabs/Monster/Minions/Dip.prefab",
        "Assets/AssetsPackge/Prefabs/Monster/Elite",
        "Assets/AssetsPackge/Prefabs/Monster/Boss/The Duke Of Flies.prefab",
    };
    string[] obstaclesPrefabPath = new[]
    {
        "Assets/Prefabs/Obstacles/Rock/Rock.prefab",
        "Assets/Prefabs/Obstacles/Spike/Spike.prefab",
        "Assets/Prefabs/Obstacles/Fire Place/Fire Place.prefab",
        "Assets/Prefabs/Obstacles/Poop/Obstacle_Poop_Normal.prefab",
        "Assets/Prefabs/Obstacles/TNT.prefab",
    };
    string[] propPrefabPath = new[]
    {
        "Assets/ScriptableObjects/Random Object/RandomChest.asset",
        "Assets/ScriptableObjects/Random Object/RandomPickup.asset",
        "Assets/ScriptableObjects/Random Object/Random Item/ItemPedestal.asset",
        "Assets/AssetsPackge/Prefabs/Props/Goods/ItemGoods.prefab",
    };

    //���ڻ���ذ徫�����ͼ����Ϊ���Ƶذ���Ҫ��д���ļ�����������
    private Sprite spriteFloor;
    private Sprite spriteWallLfet;
    private Sprite spriteWallTop;
    private Texture2D roomTexture;
    Rect previewRect = new Rect();
    Vector2 previewTextureSize = new Vector2();
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

    private void OnEnable()
    {
        rewardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(editorDefaultResourcesAssetPath[0]);
        auxiliaryLineSprite = AssetDatabase.LoadAssetAtPath<Sprite>(editorDefaultResourcesAssetPath[1]);
        emptyTexture = new Texture2D(StaticData.RoomWidthPixels, StaticData.RoomHeightPixels);

        roomEditorWindow = GetWindow<RoomEditorWindow>();
    }

    private void OnGUI()
    {
        DrawFileOperationArea();

        GUILayout.BeginVertical("Box");
        DrawFileSelectionArea();

        if (roomLayout != null) { DrawEditArea(); }
        GUILayout.EndVertical();

        if (roomLayout != null) { DrawPreviewArea(); }
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
                AssetDatabase.Refresh();
            }
        }
        GUILayout.EndHorizontal();

        if (isExpandLoadButton)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("��ͨ")) { roomLayout = SelectObject(ResourcesLoader.GetNormalRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("BOSS")) { roomLayout = SelectObject(ResourcesLoader.GetBossRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("����")) { roomLayout = SelectObject(ResourcesLoader.GetTreasureRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("�̵�")) { roomLayout = SelectObject(ResourcesLoader.GetShopRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("����")) { roomLayout = SelectObject(ResourcesLoader.GetTestRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            GUILayout.EndHorizontal();
        }
        if (isExpandCreateButton)
        {
            GUILayout.BeginVertical();
            newFileName = EditorGUILayout.TextField(newFileName);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("��ͨ")) { CreateRoomLayoutFile(ResourcesLoader.GetNormalRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("BOSS")) { CreateRoomLayoutFile(ResourcesLoader.GetBossRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("����")) { CreateRoomLayoutFile(ResourcesLoader.GetTreasureRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("�̵�")) { CreateRoomLayoutFile(ResourcesLoader.GetShopRoomPath()); isExpandCreateButton = false; }
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
        roomLayout.SpriteTop = (Sprite)EditorGUILayout.ObjectField("�Ϸ�ǽ", roomLayout.SpriteTop, typeof(Sprite), false);
        roomLayout.SpriteLeft = (Sprite)EditorGUILayout.ObjectField("���ǽ", roomLayout.SpriteLeft, typeof(Sprite), false);
        roomLayout.SpriteTip = (Sprite)EditorGUILayout.ObjectField("Tip", roomLayout.SpriteTip, typeof(Sprite), false);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        roomLayout.IsGenerateReward = GUILayout.Toggle(roomLayout.IsGenerateReward, "���ɽ���Ʒ");
        IsDrawRewardPosition = GUILayout.Toggle(IsDrawRewardPosition, "����") && roomLayout.IsGenerateReward;
        GUILayout.Space(10);
        GUILayout.Label("X");
        int x = EditorGUILayout.IntSlider((int)roomLayout.RewardPosition.x, 1, StaticData.ROOM_EDITOR_WINDOW_MAX_X);
        GUILayout.Space(10);
        GUILayout.Label("Y");
        int y = EditorGUILayout.IntSlider((int)roomLayout.RewardPosition.y, 1, StaticData.ROOM_EDITOR_WINDOW_MAX_Y);
        roomLayout.RewardPosition = new GameCoordinate(x, y);
        GUILayout.EndHorizontal();
    }
    private void EditObstacles()
    {
        //����ѡ���ļ���
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("��ʯ")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[0]); }
        if (GUILayout.Button("���")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[1]); }
        if (GUILayout.Button("���")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[2]); }
        if (GUILayout.Button("ʺ��")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[3]); }
        if (GUILayout.Button("TNT")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[4]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        //�����ļ����ƹ���͹�������ı༭����
        EditObjectList(roomLayout.obstacleList);
    }

    private void EditMonster()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("��ͨ")) { SelectAndAddObject(roomLayout.obstacleList, monsterPrefabPath[0]); }
        if (GUILayout.Button("��Ӣ")) { SelectAndAddObject(roomLayout.obstacleList, monsterPrefabPath[1]); }
        if (GUILayout.Button("Boss")) { SelectAndAddObject(roomLayout.obstacleList, monsterPrefabPath[2]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditObjectList(roomLayout.monsterList);
    }
    private void EditorProp()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("����")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[0]); }
        if (GUILayout.Button("���ʰȡ��")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[1]); }
        if (GUILayout.Button("�������")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[2]); }
        if (GUILayout.Button("�����Ʒ")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[3]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditObjectList(roomLayout.propList);
    }

    private void DrawPreviewArea()
    {
        //���Ƶذ壬���ذ徫�鲻һ����Ϊ��ʱ�����µĵذ���ͼ��Ϊ��ʱ�����հ���ͼ
        if (roomLayout.SpriteFloor != null)
        {
            var isChanged = false;
            if (spriteFloor != roomLayout.SpriteFloor)
            {
                spriteFloor = roomLayout.SpriteFloor;
                isChanged = true;
            }
            if (spriteWallTop != roomLayout.SpriteTop)
            {
                spriteWallTop = roomLayout.SpriteTop;
                isChanged = true;
            }
            if (spriteWallLfet != roomLayout.SpriteLeft)
            {
                spriteWallLfet = roomLayout.SpriteLeft;
                isChanged = true;
            }
            if (isChanged)
                roomTexture = GetRoomTexture(spriteFloor, roomLayout.SpriteTop, spriteWallLfet);

            Vector2 outset = GUILayoutUtility.GetLastRect().position + new Vector2(3, GUILayoutUtility.GetLastRect().height + 10);
            GUILayout.BeginHorizontal();
            previewTextureSize.x = roomEditorWindow.position.width - 12;
            previewTextureSize.y = previewTextureSize.x / ((float)emptyTexture.width / emptyTexture.height);
            previewRect.position = outset;
            previewRect.size = previewTextureSize;
            GUI.DrawTexture(previewRect, roomTexture);
            //GUI.Label(new Rect(outset, new Vector2(100, 20)), "Last Rect");
            GUILayout.EndHorizontal();
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
                GameCoordinate coordinate = roomLayout.RewardPosition;
                DrawSprite(rewardSprite, coordinate);
            }
            //���Ƹ�����
            if (isDrawAuxiliaryLine)
            {
                DrawSprite(auxiliaryLineSprite, centerCoordinate);
            }
        }
    }

    private void SelectAndAddObject(List<TupleWithGameObjectCoordinate> list, string path)
    {
        var @object = SelectObject(path) as GameObject;
        if (@object != null)
            AddPrefab(list, @object, centerCoordinate);
    }

    private void SelectAndAddObject(List<TupleWithRandomPickupCoordinate> list, string path)
    {
        var randomPickup = SelectObject(path) as RandomObjectSO;
        if (randomPickup != null)
            AddPrefab(list, randomPickup, centerCoordinate);
    }

    private void AddPrefab(List<TupleWithGameObjectCoordinate> list, GameObject prefab, GameCoordinate coordinate)
    {
        list.Add(new TupleWithGameObjectCoordinate(prefab, coordinate));
    }

    private void AddPrefab(List<TupleWithRandomPickupCoordinate> list, RandomObjectSO prefab, GameCoordinate coordinate)
    {
        list.Add(new TupleWithRandomPickupCoordinate(prefab, coordinate));
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
        RoomLayoutSO go = CreateInstance<RoomLayoutSO>();
        StringBuilder sb = new StringBuilder(newFileName + EXTENSION);
        string newPath = Path.Combine(path, sb.ToString());
        int cnt = 0;
        if (File.Exists(newPath))
        {
            cnt++;
            sb.Insert(newFileName.Length, string.Format(" ({0})", cnt));
            newPath = Path.Combine(path, sb.ToString());
        }
        while (File.Exists(newPath))
        {
            //sb[sb.Length - (EXTENSION.Length - 1) - 1] = (char)cnt++;
            sb.Replace((char)cnt, (char)++cnt, newFileName.Length + 2, 1);
            newPath = Path.Combine(path, sb.ToString());
        }
        AssetDatabase.CreateAsset(go, newPath);
        //roomLayout = SelectObject(newPath) as RoomLayoutSO;
        Selection.activeObject = roomLayout = go;
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
    private void EditObjectList(List<TupleWithGameObjectCoordinate> prefabs)
    {
        scrollViewVector2 = GUILayout.BeginScrollView(scrollViewVector2, GUILayout.Height(160));
        for (int i = 0; i < prefabs.Count; i++)
        {
            GUILayout.BeginHorizontal();
            prefabs[i].value1 = (GameObject)EditorGUILayout.ObjectField(prefabs[i].value1, typeof(GameObject), false);
            GUILayout.Space(30);
            GUILayout.Label("X");
            int x = EditorGUILayout.IntSlider((int)prefabs[i].value2.x, 1, StaticData.ROOM_EDITOR_WINDOW_MAX_X);
            GUILayout.Space(10);
            GUILayout.Label("Y");
            int y = EditorGUILayout.IntSlider((int)prefabs[i].value2.y, 1, StaticData.ROOM_EDITOR_WINDOW_MAX_Y);
            prefabs[i].value2 = new GameCoordinate(x, y);
            GUILayout.Space(10);
            if (GUILayout.Button("�Ƴ�")) { prefabs.RemoveAt(i); }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("���", GUILayout.MaxWidth(75)))
        {
            if (prefabs.Count == 0) AddPrefab(prefabs, null, centerCoordinate);
            else AddPrefab(prefabs, prefabs[prefabs.Count - 1].value1, prefabs[prefabs.Count - 1].value2);
            //else prefabs.Add(new TupleWithGameObjectCoordinate(prefabs[prefabs.Count - 1].value1, prefabs[prefabs.Count - 1].value2));
        }
        GUILayout.EndScrollView();
    }

    private void EditObjectList(List<TupleWithRandomPickupCoordinate> prefabs)
    {
        scrollViewVector2 = GUILayout.BeginScrollView(scrollViewVector2, GUILayout.Height(160));
        for (int i = 0; i < prefabs.Count; i++)
        {
            GUILayout.BeginHorizontal();
            prefabs[i].value1 = EditorGUILayout.ObjectField(prefabs[i].value1, typeof(RandomObjectSO), false) as RandomObjectSO;
            GUILayout.Space(30);
            GUILayout.Label("X");
            int x = EditorGUILayout.IntSlider((int)prefabs[i].value2.x, 1, StaticData.ROOM_EDITOR_WINDOW_MAX_X);
            GUILayout.Space(10);
            GUILayout.Label("Y");
            int y = EditorGUILayout.IntSlider((int)prefabs[i].value2.y, 1, StaticData.ROOM_EDITOR_WINDOW_MAX_Y);
            prefabs[i].value2 = new GameCoordinate(x, y);
            GUILayout.Space(10);
            if (GUILayout.Button("�Ƴ�")) { prefabs.RemoveAt(i); }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("���", GUILayout.MaxWidth(75)))
        {
            if (prefabs.Count == 0) AddPrefab(prefabs, null, centerCoordinate);
            else AddPrefab(prefabs, prefabs[prefabs.Count - 1].value1, prefabs[prefabs.Count - 1].value2);
        }
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
    private void DrawObjectList(List<TupleWithGameObjectCoordinate> prefabs)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].value1 != null)
            {
                var prefab = prefabs[i].value1.GetComponent<IDisplayInEditorWindow>();
                if (prefab == null)
                    prefab = prefabs[i].value1.GetComponentInChildren<IDisplayInEditorWindow>();
                var sprite = prefab.SpriteInEditorWindow;
                GameCoordinate coordinate = prefabs[i].value2;
                DrawSprite(sprite, coordinate);
            }
        }
    }
    private void DrawObjectList(List<TupleWithRandomPickupCoordinate> prefabs)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].value1 != null)
            {
                Sprite sprite = (prefabs[i].value1 as IDisplayInEditorWindow).SpriteInEditorWindow;
                GameCoordinate coordinate = prefabs[i].value2;
                DrawSprite(sprite, coordinate);
            }
        }
    }

    /// <summary>
    /// ���Ƶ������飻���ȡ�������ͼ���ٸ��ݾ����Rect����ͼ���ȡ�������
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="coordinate"></param>
    private void DrawSprite(Sprite sprite, GameCoordinate coordinate)
    {
        if (sprite == null) { Debug.Log("ͼƬΪ��"); return; }

        //�������λ��
        //���=��ǰBox��Rect(���Ͻ�,��Box�����)+��Ե����
        Vector2 outset = GUILayoutUtility.GetLastRect().position + new Vector2(3, 3);
        //���ĵ�=���+�ذ���ͼ��С/2
        Vector2 center = outset + previewRect.size / 2;
        var scale = previewRect.width / emptyTexture.width;
        //GUI.Label(new Rect(center, new Vector2(150, 20)), string.Format("Draw center {0}", scale));
        //����λ��=���ĵ�+ƫ��(����*���ش�С)-�����һ���С(����������λ�����Ͻǣ���ȥ�����С��һ��ʹ����ʾʱ����������ĵ���ǰ������λ��)
        int UnitPixels = (int)(StaticData.RoomHorizontalUnitSize * 100 / 2 * scale);
        Vector2 pos = center - new Vector2(StaticData.RoomHorizontalUnit - coordinate.x,
                                           StaticData.RoomVerticalUnit - coordinate.y) * UnitPixels
                             - sprite.rect.size / 2 * scale;
        //���û��Ƶ�λ�úʹ�С
        Rect displayArea = sprite.rect;
        Rect newRect = new Rect(pos, displayArea.size * scale);

        //��Ϊ4��������СҪ��Ϊ0-1֮�䣬���Գ���ԭ��ͼ���õ�����
        var tex = sprite.texture;
        displayArea.xMin /= tex.width;
        displayArea.xMax /= tex.width;
        displayArea.yMin /= tex.height;
        displayArea.yMax /= tex.height;

        //���������ֱ�Ϊ:���Ƶ�λ�úʹ�С,ԭ��ͼ��ԭ��ͼ��ȡ������
        GUI.DrawTextureWithTexCoords(newRect, tex, displayArea);
        //GUI.Label(new Rect(pos * scale, new Vector2(200, 20)), string.Format("Draw texture {0}", (pos * scale).ToString()));
    }

    /// <summary>
    /// ����������3����ת��ͼ������Ϊһ����ͼ����
    /// </summary>
    /// <param name="spriteFloor"></param>
    /// <returns></returns>
    private Texture2D GetRoomTexture(Sprite spriteFloor, Sprite spriteTop, Sprite spriteLeft)
    {
        var result = new Texture2D(StaticData.RoomWidthPixels, StaticData.RoomHeightPixels);
        result.SetPixels((result.width - spriteFloor.texture.width) / 2,
                         (result.height - spriteFloor.texture.height) / 2,
                         spriteFloor.texture.width,
                         spriteFloor.texture.height,
                         spriteFloor.texture.GetPixels());

        if (spriteLeft != null)
        {
            Texture2D leftTexture = spriteLeft.texture;
            int leftTextureWidth = leftTexture.width;
            int leftTextureHeight = leftTexture.height;
            var rightTexture = new Texture2D(leftTextureWidth, leftTextureHeight);
            for (int i = 0; i < leftTextureWidth; i++)
            {
                rightTexture.SetPixels(i, 0, 1, leftTextureHeight, leftTexture.GetPixels(leftTextureWidth - 1 - i, 0, 1, leftTextureHeight));
            }
            result.SetPixels(0, 0, leftTextureWidth, leftTextureHeight, leftTexture.GetPixels());

            result.SetPixels(result.width - rightTexture.width,
                             0,
                             rightTexture.width,
                             rightTexture.height,
                             rightTexture.GetPixels());
        }
        if (spriteTop != null)
        {
            Texture2D topTexture = spriteTop.texture;
            int topTextureWidth = topTexture.width;
            int topTextureHeight = topTexture.height;
            var bottomTexture = new Texture2D(topTextureWidth, topTextureHeight);
            for (int i = 0; i < topTextureHeight; i++)
            {
                bottomTexture.SetPixels(0, i, topTextureWidth, 1, topTexture.GetPixels(0, topTextureHeight - 1 - i, topTextureWidth, 1));
            }

            Color pixel;
            for (int y = 0; y < topTextureHeight; y++)
            {
                for (int x = 0; x < topTextureWidth; x++)
                {
                    pixel = bottomTexture.GetPixel(x, y);
                    if (pixel.a != 0f)
                        result.SetPixel((result.width - topTextureWidth) / 2 + x,
                                        y,
                                        pixel);

                    pixel = topTexture.GetPixel(x, y);
                    if (pixel.a != 0f)
                        result.SetPixel((result.width - topTextureWidth) / 2 + x,
                                        result.height - topTextureHeight + y,
                                        pixel);
                }
            }
        }

        result.Apply(true);
        return result;
    }
}
