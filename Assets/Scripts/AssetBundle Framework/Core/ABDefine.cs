using System.IO;
using UnityEngine;

namespace AssetBundleFramework
{
    public class ABDefine
    {
        /// <summary>
        /// assetbundle �Ĵ��λ��
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
        /// ���AB�����ļ�������
        /// </summary>
        /// <returns></returns>
#if UNITY_EDITOR
        public static string GetIniPath()
        {
            return GetABPackPath("�����ļ�/ABIni.json");
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