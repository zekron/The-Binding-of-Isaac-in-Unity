using Newtonsoft.Json;
using System.IO;
using UnityEditor;

namespace AssetBundleFramework
{
    public static class ABEditor
    {
        static ABIni Ini = new ABIni();
        /// <summary>
        /// 自动设置AB包标签
        /// 同时生成对应的配置文件
        /// </summary>
        [MenuItem("ABFW/自动标签")]
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
        private static void AutoSetABLabel(string filePath, int index, string abPrefix = null, string abSuffix = null)
        {
            if (File.Exists(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                string resName; //配置文件内资源的名称
                if (fi.Extension == ".meta")
                    return;
                filePath = filePath.Substring(filePath.LastIndexOf("Assets"));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                if (fi.Extension == ".unity")
                {
                    ai.assetBundleName = abPrefix + "/" + abPrefix;
                    ai.assetBundleVariant = "u3d";
                    resName = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
                }
                else
                {
                    ai.assetBundleName = abPrefix + "/" + abSuffix;
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
        }

        [MenuItem("ABFW/创建AB包")]
        static void BuildAB()
        {
            string abPath = ABEditorTools.Instance.SaveABPackPath;
            BuildPipeline.BuildAssetBundles(abPath, BuildAssetBundleOptions.ChunkBasedCompression, ABEditorTools.Instance.BudTar);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("ABFW/清空AB")]
        static void Clear()
        {
            if (Directory.Exists(ABEditorTools.Instance.SaveABPackPath))
            {
                Directory.Delete(ABEditorTools.Instance.SaveABPackPath, true);
                AssetDatabase.Refresh();
            }
        }
    }
}
