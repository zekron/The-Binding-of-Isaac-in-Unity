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
            if (AutoSetABLabel(ABEditorTools.Instance.ABResPath, 0))
            {
                Debug.Log("Successfully auto set labels.");
                //生成配置文件
                File.WriteAllText(ABEditorTools.Instance.IniFilePath, JsonConvert.SerializeObject(Ini));
                //清除记录
                Ini.Clear();
            }
            else
                Debug.LogError("Failed to auto set labels.");
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
            if (AutoSetABLabel(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]).Replace('/', '\\'), 1))
            {
                Debug.Log("Successfully auto set labels.");
                //生成配置文件
                File.WriteAllText(ABEditorTools.Instance.IniFilePath, JsonConvert.SerializeObject(Ini));
                //清除记录
            }
            else
                Debug.LogError("Failed to auto set labels.");
            Ini.Clear();
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
        /// <param name="suffix"></param>
        static void RenameFiles(string filePath, string suffix)
        {
            if (File.Exists(filePath))
            {
                string directory = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                if (fileName.Contains(suffix)) return;
                if (fileName.Contains(".meta") && !fileName.Remove(fileName.IndexOf(".meta")).Contains(".")) return;
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
        private static bool AutoSetABLabel(string filePath, int index, string abPrefix = null, string abSuffix = null)
        {
            if (File.Exists(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                if (fi.Name[0] == '.') return false;
                if (fi.Extension == ".meta") return false;

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
                return true;
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
                else if (index == 3)
                {
                    abPrefix = Path.Combine(abPrefix, abSuffix);
                    abSuffix = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                }

                DirectoryInfo di = new DirectoryInfo(filePath);
                FileSystemInfo[] fsInfos = di.GetFileSystemInfos();
                for (int i = 0; i < fsInfos.Length; i++)
                {
                    AutoSetABLabel(fsInfos[i].FullName, index + 1, abPrefix, abSuffix);
                }
                return true;
            }
            else
            {
                Debug.LogWarning($"Directory <color=yellow>{filePath}</color> is not exist. Created.");
                Directory.CreateDirectory(filePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return false;
            }
        }
    }
}
