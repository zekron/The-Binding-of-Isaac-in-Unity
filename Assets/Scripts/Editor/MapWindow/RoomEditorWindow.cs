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

    string newFileName = "我是文件名";

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

    //用于缓存地板精灵和贴图，因为绘制地板需要读写新文件，开销过大
    private Sprite spriteFloor;
    private Sprite spriteWallLfet;
    private Sprite spriteWallTop;
    private Texture2D roomTexture;
    Rect previewRect = new Rect();
    Vector2 previewTextureSize = new Vector2();
    //用于没有地板时绘制空白区域
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
        if (GUILayout.Button("打开文件"))
        {
            isExpandLoadButton = !isExpandLoadButton; isExpandCreateButton = false;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("创建文件"))
        {
            isExpandCreateButton = !isExpandCreateButton; isExpandLoadButton = false;
        }
        GUILayout.Space(10);
        if (GUILayout.Button("保存修改"))
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
            if (GUILayout.Button("普通")) { roomLayout = SelectObject(ResourcesLoader.GetNormalRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("BOSS")) { roomLayout = SelectObject(ResourcesLoader.GetBossRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("宝藏")) { roomLayout = SelectObject(ResourcesLoader.GetTreasureRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("商店")) { roomLayout = SelectObject(ResourcesLoader.GetShopRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            if (GUILayout.Button("测试")) { roomLayout = SelectObject(ResourcesLoader.GetTestRoomAsset()) as RoomLayoutSO; isExpandLoadButton = false; }
            GUILayout.EndHorizontal();
        }
        if (isExpandCreateButton)
        {
            GUILayout.BeginVertical();
            newFileName = EditorGUILayout.TextField(newFileName);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("普通")) { CreateRoomLayoutFile(ResourcesLoader.GetNormalRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("BOSS")) { CreateRoomLayoutFile(ResourcesLoader.GetBossRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("宝藏")) { CreateRoomLayoutFile(ResourcesLoader.GetTreasureRoomPath()); isExpandCreateButton = false; }
            if (GUILayout.Button("商店")) { CreateRoomLayoutFile(ResourcesLoader.GetShopRoomPath()); isExpandCreateButton = false; }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void DrawFileSelectionArea()
    {
        GUILayout.BeginHorizontal("box");
        RoomLayoutSO temp = roomLayout;
        roomLayout = (RoomLayoutSO)EditorGUILayout.ObjectField("文件", roomLayout, typeof(RoomLayoutSO), false);
        GUILayout.Space(50);
        isDrawAuxiliaryLine = GUILayout.Toggle(isDrawAuxiliaryLine, "辅助线");
        //监控roomLayout文件是否被替换
        if (temp != roomLayout) { ResetEditInstallWhenChange(); }
        GUILayout.EndHorizontal();
    }

    private void DrawEditArea()
    {
        GUILayout.Space(5);
        toolBarNum = GUILayout.Toolbar(toolBarNum, new[] { "主要", "障碍物列表", "怪物列表", "道具列表" });

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
        roomLayout.SpriteFloor = (Sprite)EditorGUILayout.ObjectField("地板", roomLayout.SpriteFloor, typeof(Sprite), false);
        roomLayout.SpriteTop = (Sprite)EditorGUILayout.ObjectField("上方墙", roomLayout.SpriteTop, typeof(Sprite), false);
        roomLayout.SpriteLeft = (Sprite)EditorGUILayout.ObjectField("左侧墙", roomLayout.SpriteLeft, typeof(Sprite), false);
        roomLayout.SpriteTip = (Sprite)EditorGUILayout.ObjectField("Tip", roomLayout.SpriteTip, typeof(Sprite), false);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        roomLayout.IsGenerateReward = GUILayout.Toggle(roomLayout.IsGenerateReward, "生成奖励品");
        IsDrawRewardPosition = GUILayout.Toggle(IsDrawRewardPosition, "绘制") && roomLayout.IsGenerateReward;
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
        //快速选择文件夹
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("岩石")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[0]); }
        if (GUILayout.Button("尖刺")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[1]); }
        if (GUILayout.Button("火堆")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[2]); }
        if (GUILayout.Button("屎堆")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[3]); }
        if (GUILayout.Button("TNT")) { SelectAndAddObject(roomLayout.obstacleList, obstaclesPrefabPath[4]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        //根据文件绘制怪物和怪物坐标的编辑区域
        EditObjectList(roomLayout.obstacleList);
    }

    private void EditMonster()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("普通")) { SelectAndAddObject(roomLayout.obstacleList, monsterPrefabPath[0]); }
        if (GUILayout.Button("精英")) { SelectAndAddObject(roomLayout.obstacleList, monsterPrefabPath[1]); }
        if (GUILayout.Button("Boss")) { SelectAndAddObject(roomLayout.obstacleList, monsterPrefabPath[2]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditObjectList(roomLayout.monsterList);
    }
    private void EditorProp()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("箱子")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[0]); }
        if (GUILayout.Button("随机拾取物")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[1]); }
        if (GUILayout.Button("随机道具")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[2]); }
        if (GUILayout.Button("随机商品")) { SelectAndAddObject(roomLayout.propList, propPrefabPath[3]); }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);

        EditObjectList(roomLayout.propList);
    }

    private void DrawPreviewArea()
    {
        //绘制地板，当地板精灵不一样或为空时制作新的地板贴图，为空时创建空白贴图
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

        //绘制房间内的物体
        if (Event.current.type == EventType.Repaint)
        {
            //绘制tip
            if (roomLayout.SpriteTip != null) { DrawSprite(roomLayout.SpriteTip, centerCoordinate); }
            //绘制障碍物
            DrawObjectList(roomLayout.obstacleList);
            //绘制怪物
            DrawObjectList(roomLayout.monsterList);
            //绘制道具
            DrawObjectList(roomLayout.propList);
            //绘制奖励位置
            if (IsDrawRewardPosition)
            {
                GameCoordinate coordinate = roomLayout.RewardPosition;
                DrawSprite(rewardSprite, coordinate);
            }
            //绘制辅助线
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
    /// 根据路径选择Asset下的文件或文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private UnityEngine.Object SelectObject(string path)
    {
        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);
        if (obj == null) { Debug.Log("文件不存在： " + path); return null; }
        Selection.activeObject = obj;
        return obj;
    }

    /// <summary>
    /// 创建新的布局文件并选择该文件
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
    /// 根据预制体和坐标绘制编辑行列，有高度限制(170)
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
            if (GUILayout.Button("移除")) { prefabs.RemoveAt(i); }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("添加", GUILayout.MaxWidth(75)))
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
            if (GUILayout.Button("移除")) { prefabs.RemoveAt(i); }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("添加", GUILayout.MaxWidth(75)))
        {
            if (prefabs.Count == 0) AddPrefab(prefabs, null, centerCoordinate);
            else AddPrefab(prefabs, prefabs[prefabs.Count - 1].value1, prefabs[prefabs.Count - 1].value2);
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 更换RoomLayout时重置相关设置
    /// </summary>
    private void ResetEditInstallWhenChange()
    {
        toolBarNum = 0;
        isDrawAuxiliaryLine = false;
        IsDrawRewardPosition = true;
    }

    /// <summary>
    /// 新建RoomLayout时重置相关设置
    /// </summary>
    private void ResetEditInstallWhenCreate()
    {
        toolBarNum = 0;
        isDrawAuxiliaryLine = true;
        IsDrawRewardPosition = true;
    }

    /// <summary>
    /// 根据预制体和坐标绘制预览图像
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
    /// 绘制单个精灵；需获取精灵的贴图，再根据精灵的Rect在贴图里截取区域绘制
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="coordinate"></param>
    private void DrawSprite(Sprite sprite, GameCoordinate coordinate)
    {
        if (sprite == null) { Debug.Log("图片为空"); return; }

        //计算绘制位置
        //起点=当前Box的Rect(左上角,该Box的起点)+边缘留白
        Vector2 outset = GUILayoutUtility.GetLastRect().position + new Vector2(3, 3);
        //中心点=起点+地板贴图大小/2
        Vector2 center = outset + previewRect.size / 2;
        var scale = previewRect.width / emptyTexture.width;
        //GUI.Label(new Rect(center, new Vector2(150, 20)), string.Format("Draw center {0}", scale));
        //绘制位置=中心点+偏移(坐标*像素大小)-精灵的一半大小(精灵绘制起点位于左上角，减去精灵大小的一半使得显示时：精灵的中心等于前面计算的位置)
        int UnitPixels = (int)(StaticData.RoomHorizontalUnitSize * 100 / 2 * scale);
        Vector2 pos = center - new Vector2(StaticData.RoomHorizontalUnit - coordinate.x,
                                           StaticData.RoomVerticalUnit - coordinate.y) * UnitPixels
                             - sprite.rect.size / 2 * scale;
        //设置绘制的位置和大小
        Rect displayArea = sprite.rect;
        Rect newRect = new Rect(pos, displayArea.size * scale);

        //因为4个参数大小要求为0-1之间，所以除以原贴图，得到比例
        var tex = sprite.texture;
        displayArea.xMin /= tex.width;
        displayArea.xMax /= tex.width;
        displayArea.yMin /= tex.height;
        displayArea.yMax /= tex.height;

        //三个参数分别为:绘制的位置和大小,原贴图，原贴图截取的区域
        GUI.DrawTextureWithTexCoords(newRect, tex, displayArea);
        //GUI.Label(new Rect(pos * scale, new Vector2(200, 20)), string.Format("Draw texture {0}", (pos * scale).ToString()));
    }

    /// <summary>
    /// 制作参数的3个翻转贴图，并合为一张贴图返回
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
