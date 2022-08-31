using System.IO;
using UnityEngine;

namespace AssetBundleFramework
{
    public class ABDefine
    {
        /// <summary>
        /// assetbundle 的存放位置
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetABPackPath(string fileName = null)
        {
            return Path.Combine(Application.streamingAssetsPath, "AssetBundles/PC", fileName ?? "PC");
        }
#elif UNITY_STANDALONE
        public static string GetABPackPath(string fileName=null)
        {
            return Path.Combine(Application.persistentDataPath,"AssetBundles/PC", fileName ?? "PC");
        }
#elif UNITY_ANDROID
        public static string GetABPackPath(string fileName=null)
        {
            return Path.Combine(Application.persistentDataPath,"AssetBundles/Android", fileName ?? "Android");
        }
#elif UNITY_IPHONE
        public static string GetABPackPath(string fileName=null)
        {
            return Path.Combine(Application.persistentDataPath,"AssetBundles/IOS", fileName ?? "IOS");
        }
#endif
        /// <summary>
        /// 获得AB配置文件包名称
        /// </summary>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetIniPath()
        {
            return GetABPackPath("配置文件/ABIni.json");
        }
#elif UNITY_STANDALONE
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "AssetBundles", "配置文件/ABIni.json");
        }
#elif UNITY_ANDROID
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "AssetBundles", "配置文件/ABIni.json");
        }
#elif UNITY_IPHONE
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "AssetBundles", "配置文件/ABIni.json");
        }
#endif
    }
}