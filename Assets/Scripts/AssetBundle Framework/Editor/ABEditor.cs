using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundleFramework
{
    public static class ABEditor
    {
        static ABIni Ini = new ABIni();
        /// <summary>
        /// 自动设置AB包标签
        /// 同时生成对应的配置文件
        /// </summary>
        [MenuItem("AssetBundle Framework/自动标签")]
        static void AutoSetABLabel()
        {
            //移除无用标签
            AssetDatabase.RemoveUnusedAssetBundleNames();
            //自动设置标签
            AutoSetABLabel(ABEditorTools.Instance.ABResPath, 0);
            //生成配置文件
            File.WriteAllText(ABEditorTools.Instance.IniFilePath, JsonConvert.SerializeObject(Ini));
            //清除记录
            Ini.Clear();
        }
        [MenuItem("Assets/Custom Menu/Build AssetBundle(Experimental)/Windows")]
        static void SetABLabelWithSelectedFolder()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            if (assetGUIDs.Length > 1)
            {
                Debug.LogError($"Current selections <color=red>{assetGUIDs.Length}</color> are more than 1.");
                return;
            }

            //恢复已有数据
            Ini = JsonConvert.DeserializeObject<ABIni>(File.ReadAllText(ABEditorTools.Instance.IniFilePath));
            AutoSetABLabel(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]).Replace('/', '\\'), 1);
            //生成配置文件
            File.WriteAllText(ABEditorTools.Instance.IniFilePath, JsonConvert.SerializeObject(Ini));
            //清除记录
            Ini.Clear();
        }

        [MenuItem("AssetBundle Framework/创建AB包")]
        static void BuildAB()
        {
            string abPath = ABEditorTools.Instance.SaveABPackPath;
            BuildPipeline.BuildAssetBundles(abPath, BuildAssetBundleOptions.ChunkBasedCompression, ABEditorTools.Instance.BudTar);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("AssetBundle Framework/清空AB")]
        static void Clear()
        {
            if (Directory.Exists(ABEditorTools.Instance.SaveABPackPath))
            {
                Directory.Delete(ABEditorTools.Instance.SaveABPackPath, true);
                AssetDatabase.Refresh();
            }
        }
        private static void AutoSetABLabel(string filePath, int index, string abPrefix = null, string abSuffix = null)
        {
            if (File.Exists(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                if (fi.Extension == ".meta") return;

                string resName; //配置文件内资源的名称
                filePath = filePath.Substring(filePath.LastIndexOf("Assets"));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                if (fi.Extension == ".unity")
                {
                    ai.assetBundleName = abSuffix == null ? abPrefix : $"{abPrefix}/{abSuffix}";
                    ai.assetBundleVariant = "u3d";
                    resName = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
                }
                else
                {
                    ai.assetBundleName = abSuffix == null ? abPrefix : $"{abPrefix}/{abSuffix}";
                    ai.assetBundleVariant = "ab";
                    resName = fi.Name;
                }
                //添加配置文件
                Ini.AddData(resName, ai.assetBundleName + "." + ai.assetBundleVariant);
            }
            else if (Directory.Exists(filePath))
            {
                if (index == 1)
                {
                    abPrefix = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                }
                else if (index == 2)
                {
                    abSuffix = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                }

                DirectoryInfo di = new DirectoryInfo(filePath);
                FileSystemInfo[] fsInfos = di.GetFileSystemInfos();
                for (int i = 0; i < fsInfos.Length; i++)
                {
                    AutoSetABLabel(fsInfos[i].FullName, index + 1, abPrefix, abSuffix);
                }
            }
            else
            {
                Debug.LogWarning($"Directory <color=yellow>{filePath}</color> is not exist. Created.");
                Directory.CreateDirectory(filePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
