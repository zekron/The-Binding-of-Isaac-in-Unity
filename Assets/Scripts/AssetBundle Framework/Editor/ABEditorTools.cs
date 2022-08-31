using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// 发布的平台
    /// 路径:
    ///     AB资源的位置
    ///     打包AB包的保存位置
    /// </summary>
    public class ABEditorTools
    {
        private static ABEditorTools _Instance = new ABEditorTools();
        public static ABEditorTools Instance
        {
            get
            {
                return _Instance;
            }
        }
        private ABEditorTools()
        {
            BudTar = EditorUserBuildSettings.activeBuildTarget;
            Debug.Log(BudTar);
            ABResPath = "Assets/ABRes";
        }
        public BuildTarget BudTar;
        public string ABResPath;
        //AB包的打包保存路径
        private string _SaveABPackPath = null;
        public string SaveABPackPath
        {
            get
            {
                if (_SaveABPackPath == null)
                {
                    _SaveABPackPath = GetABPackPath();
                }
                return _SaveABPackPath;
            }
        }
        //配置文件路径
        private string _IniFilePath = null;
        public string IniFilePath
        {
            get
            {
                if (_IniFilePath == null)
                {
                    _IniFilePath = GetIniPath();
                }
                return _IniFilePath;
            }
        }
        /// <summary>
        /// 获得配置文件路径
        /// </summary>
        /// <returns></returns>
        private string GetIniPath()
        {
            string dir = GetABPackPath() + "/配置文件";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filePath = Path.Combine(dir, "ABIni.json");
            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return filePath;
        }
        /// <summary>
        /// 获得AB包打包的保存路径
        /// </summary>
        /// <returns></returns>
        private string GetABPackPath()
        {
            string path = string.Empty;
            switch (BudTar)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    path = Path.Combine(Application.streamingAssetsPath, "AssetBundles/PC");
                    break;
                case BuildTarget.iOS:
                    path = Path.Combine(Application.streamingAssetsPath, "AssetBundles/IOS");
                    break;
                case BuildTarget.Android:
                    path = Path.Combine(Application.streamingAssetsPath, "AssetBundles/Android");
                    break;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return path;
        }
    }
}

