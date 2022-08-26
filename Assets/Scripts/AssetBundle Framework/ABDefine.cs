using System.IO;
using UnityEngine;

namespace AssetBundleFramework
{
    public class ABDefine
    {
        /// <summary>
        /// assetbundle �Ĵ��λ��
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetABPackPath(string abName = null)
        {
            return Path.Combine(Application.streamingAssetsPath, "AssetBundles/PC", abName ?? "PC");
        }
#elif UNITY_STANDALONE
        public static string GetABPackPath(string abName=null)
        {
            return Path.Combine(Application.persistentDataPath,"AssetBundles/PC",abName??"PC");
        }
#elif UNITY_ANDROID
        public static string GetABPackPath(string abName=null)
        {
            return Path.Combine(Application.persistentDataPath,"AssetBundles/PC",abName??"Android");
        }
#elif UNITY_IPHONE
        public static string GetABPackPath(string abName=null)
        {
            return Path.Combine(Application.persistentDataPath,"AssetBundles/PC",abName??"IOS");
        }
#endif
        /// <summary>
        /// ���AB�����ļ�������
        /// </summary>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetIniPath()
        {
            return Path.Combine(Application.streamingAssetsPath, "AssetBundles", "�����ļ�/ABIni.json");
        }
#elif UNITY_STANDALONE
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "AssetBundles", "�����ļ�/ABIni.json");
        }
#elif UNITY_ANDROID
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "AssetBundles", "�����ļ�/ABIni.json");
        }
#elif UNITY_IPHONE
        public static string GetIniPath()
        {
            return Path.Combine(Application.persistentDataPath, "AssetBundles", "�����ļ�/ABIni.json");
        }
#endif
    }
}