using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// 泛型方法生成资源
    /// 先看资源是否存在对应的AB包中
    /// 不存在从Resources中加载
    /// 存在获得对应的AB包然后加载
    /// </summary>
    public class AssetBundleManager
    {
        private static AssetBundleManager _Instance;
        public static AssetBundleManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AssetBundleManager();
                return _Instance;
            }
        }
        /// <summary>
        /// key - 资源名称 ， value - assetbundle 名称
        /// </summary>
        private Dictionary<string, string> _ResPath;
        public AssetBundleFactory Factory;
        public AssetBundleManager()
        {
            Factory = new AssetBundleFactory();

            //初始化AB资源集合
            _ResPath = new Dictionary<string, string>();
            ABIni ini = JsonConvert.DeserializeObject<ABIni>(File.ReadAllText(ABDefine.GetIniPath()));
            if (ini != null) Debug.Log("配置文件读取成功！");
            //Debug.Log("资源与AB包对应关系如下：");
            for (int i = 0; i < ini.Datas.Count; i++)
            {
                _ResPath.Add(ini.Datas[i].ResName, ini.Datas[i].ABName);
                //Debug.Log($"{ini.Datas[i].ResName} -> {ini.Datas[i].ABName}");
            }
        }

        public void LoadAssetBundle(string abName)
        {
            if (!_ResPath.ContainsValue(abName)) Debug.LogError($"配置表中并没有登记该资产包: {abName}");

            Factory.GetABLoader(abName);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resName"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string resName) where T : Object
        {
            T t;
            if (!_ResPath.ContainsKey(resName))
            {
                //从Resouces下加载
                t = Resources.Load<T>(resName);
                Debug.Log($"<color=yellow>无法在已登记的Asset Bundles配置表中加载对应资源文件： {resName}</color>");
                Debug.Log($"现从Resources加载文件 -> {t.name}");
                return t;
            }
            //从AB中加载
            string abName = _ResPath[resName];
            ABLoader loader = Factory.GetABLoader(abName);
            //对于场景，把ab包加载进内存就足够了。
            if (abName.EndsWith(".u3d"))
            {
                return null;
            }
            t = loader.LoadAsset<T>(resName);
            if (!t)
            {
                Debug.LogError($"找不到 type = {typeof(T)}, name = {resName} 的资源文件！ ");
                return null;
            }
            Debug.Log($"<color=green>成功从Asset Bundles加载对应资源文件： {resName}</color>");
            return t;
        }
        /// <summary>
        /// 卸载某个资源
        /// </summary>
        /// <param name="resName"></param>
        public void ReleaseResources(string resName)
        {
            if (_ResPath.ContainsKey(resName))
            {
                string abname = _ResPath[resName];
                ABLoader loader = Factory.GetABLoader(abname);
                loader.UnloadResource(resName);
            }
        }
        /// <summary>
        /// 卸载所有的资源
        /// </summary>
        /// <param name="resName"></param>
        public void ReleaseAllResources()
        {
            Factory.ReleaseAllABLoader();
        }
    }
}
