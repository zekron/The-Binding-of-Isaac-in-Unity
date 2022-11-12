using CustomPhysics2D;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomEditorMenu : EditorWindow
{

    [MenuItem("Custom Menu/Window/Player Profile Editor")]
    private static void CreatePlayerWindow()
    {
        GetWindow<PlayerProfileWindow>("Player Profile Editor", true);
    }

    [MenuItem("Custom Menu/Window/Enemy Profile Editor")]
    private static void CreateEnemyWindow()
    {
        EditorWindow window = GetWindow<EnemyProfileWindow>("Enemy Profile Editor", true);
        //window.minSize = window.maxSize = new Vector2(600, 600);
    }

    [MenuItem("Custom Menu/Window/Room Editor Window", false, 128)]
    private static void CreateRoomWindow()
    {
        EditorWindow window = GetWindow<RoomEditorWindow>("Room Editor", true);
        //window.minSize = window.maxSize = new Vector2(800, 800);
    }

    [MenuItem("Custom Menu/Window/Trinket Editor Window", false, 256)]
    private static void CreateTrinketWindow()
    {
        EditorWindow window = GetWindow<TrinketItemEditorWindow>("Item Editor", true);
        //window.minSize = window.maxSize = new Vector2(800, 800);
    }

    [MenuItem("Custom Menu/Window/Collectible Editor Window", false, 256)]
    private static void CreateCollectibleWindow()
    {
        EditorWindow window = GetWindow<CollectibleItemEditorWindow>("Collectible Item Editor", true);
        //window.minSize = window.maxSize = new Vector2(800, 800);
    }

    [MenuItem("Custom Menu/Assets Bundle/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
#if UNITY_EDITOR
        var platform = "PC";
#endif
        string dir = string.Format("{0}/AssetBundles/{1}", Application.streamingAssetsPath, platform);
        //判断该目录是否存在
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);//在工程下创建AssetBundles目录
        }
        //参数一为打包到哪个路径，参数二压缩选项  参数三 平台的目标
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static string _settingsPath = "Assets/Custom Collision 2D/Resources/Custom Physics Settings.asset";
    [MenuItem("Custom Menu/Physics Settings", false, 1)]
    static void EditPhyscisSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<CustomPhysicsSetting>(_settingsPath);
        if (settings == null)
        {
            settings = CreateInstance<CustomPhysicsSetting>();
            AssetDatabase.CreateAsset(settings, _settingsPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
        }
        Selection.activeObject = settings;
        //EditorGUIUtility.PingObject( settings );
    }

    [MenuItem("Assets/Custom Menu/Rename File(s)", validate = false)]
    public static void RenameFiles()
    {
        string[] assetGUIDs = Selection.assetGUIDs;
        for (int i = 0; i < assetGUIDs.Length; i++)
        {
            //单一文件夹GUID转路径：Assets/Sprites/...
            var directory = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                FileSystemInfo[] fsInfos = di.GetFileSystemInfos(); // Assets\\Sprites\\...
                var suffix = directory.Substring(directory.LastIndexOf('/') + 1);
                for (int j = 0; j < fsInfos.Length; j++)
                {
                    RenameFiles(fsInfos[j].FullName, suffix);
                }
            }
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("Assets/Custom Menu/Rename File(s)", validate = true)]
    static bool CanRenameFiles()
    {
        string[] assetGUIDs = Selection.assetGUIDs;
        //if (assetGUIDs.Length > 1) return false;
        for (int i = 0; i < assetGUIDs.Length; i++)
        {
            if (File.Exists(AssetDatabase.GUIDToAssetPath(assetGUIDs[i]))) return false;
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">Full path: C:\\ ... \\Assets\\Sprites\\...</param>
    /// <param name="suffix">current folder name</param>
    static void RenameFiles(string filePath, string suffix)
    {
        if (File.Exists(filePath))
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            if (fileName.Contains(suffix)) return;
            if (fileName.Contains(".meta") && !fileName.Remove(fileName.IndexOf(".meta")).Contains(".")) return;    //not folder
            File.Move(filePath, $"{directory}\\{suffix}_{Path.GetFileName(filePath)}");
        }
        else if (Directory.Exists(filePath))
        {
            Debug.Log(filePath);
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileSystemInfo[] fsInfos = di.GetFileSystemInfos(); // Assets\\Sprites\\...
            suffix = filePath.Substring(filePath.LastIndexOf('\\') + 1);
            for (int j = 0; j < fsInfos.Length; j++)
            {
                RenameFiles(fsInfos[j].FullName, suffix);
            }
        }
    }

    private static string headSpriteGroupBasePath = "Assets/ABRes/ScriptableObjects/Head Sprite Group";
    [MenuItem("Assets/Custom Menu/Create Head Sprite Group")]
    static void CreateHeadSpriteGroup()
    {
        string[] assetGUIDs = Selection.assetGUIDs;
        string assetPath;
        Object[] sprites;
        HeadSpriteGroup spriteGroup;
        string fileName;

        for (int i = 0; i < assetGUIDs.Length; i++)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
            sprites = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            if (sprites.Length != 9)
            {
                Debug.LogWarning($"There's not enough sprites. Consider that Texture2D <color=yellow>{sprites[0].name}</color> has been not sliced.");
                continue;
            }

            spriteGroup = CreateInstance<HeadSpriteGroup>();
            fileName = sprites[0].name.Substring(sprites[0].name.LastIndexOf('_') + 1);

            for (int j = 1; j < sprites.Length; j++)
                spriteGroup.InitializeGroup(sprites[j] as Sprite, j);
            AssetDatabase.CreateAsset(spriteGroup, $"{headSpriteGroupBasePath}/Head Sprite Group_{fileName}.asset");
            AssetDatabase.SaveAssets();

            Selection.activeObject = spriteGroup;
        }

        EditorUtility.FocusProjectWindow();
    }
}
