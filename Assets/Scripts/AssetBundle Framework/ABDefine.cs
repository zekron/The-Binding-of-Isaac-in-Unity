using System.IO;
using UnityEngine;

namespace AssetBundleFramework
{
    public class ABDefine
    {
        /// <summary>
        /// assetbundle 的存放位置
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetABPackPath(string abName = null)
        {
            return Path.Combine(Application.streamingAssetsPath, "PC", abName ?? "PC");
        }
#elif UNITY_STANDALONE
        public static string GetABPackPath(string abName=null)
        {
            return Path.Combine(Application.persistentDataPath,"PC",abName??"PC");
        }
#elif UNITY_ANDROID
        public static string GetABPackPath(string abName=null)
        {
            return Path.Combine(Application.persistentDataPath,"Android",abName??"Android");
        }
#elif UNITY_IPHONE
        public static string GetABPackPath(string abName=null)
        {
            return Path.Combine(Application.persistentDataPath,"IOS",abName??"IOS");
        }
#endif
        /// <summary>
        /// 获得AB配置文件包名称
        /// </summary>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetIniPath()
        {
            return Path.Combine(Application.streamingAssetsPath, "PC", "配置文件/ABIni.json");
        }
#elif UNITY_STANDALONE
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "PC", "配置文件/ABIni.json");
        }
#elif UNITY_ANDROID
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "PC", "配置文件/ABIni.json");
        }
#elif UNITY_IPHONE
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "PC", "配置文件/ABIni.json");
        }
#endif
    }
}