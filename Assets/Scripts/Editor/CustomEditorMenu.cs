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

    [MenuItem("Custom Menu/Assets Bundle/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string dir = string.Format("{0}/AssetBundles", Application.streamingAssetsPath);
        //判断该目录是否存在
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);//在工程下创建AssetBundles目录
        }
        //参数一为打包到哪个路径，参数二压缩选项  参数三 平台的目标
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
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
}
