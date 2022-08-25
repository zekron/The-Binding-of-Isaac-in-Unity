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
        private Dictionary<string, string> _ResPath; //key - 资源名称 ， value - assetbundle 名称
        public AssetBundleFactory Factory;
        public AssetBundleManager()
        {
            Factory = new AssetBundleFactory();

            //初始化AB资源集合
            _ResPath = new Dictionary<string, string>();
            ABIni ini = JsonConvert.DeserializeObject<ABIni>(File.ReadAllText(ABDefine.GetIniPath()));
            Debug.Log("资源与AB包对应关系如下：");
            for (int i = 0; i < ini.Datas.Count; i++)
            {
                _ResPath.Add(ini.Datas[i].ResName, ini.Datas[i].ABName);
                Debug.Log($"{ini.Datas[i].ResName}->{ini.Datas[i].ABName}");
            }
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resName"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string resName) where T : UnityEngine.Object
        {
            T t;
            if (!_ResPath.ContainsKey(resName))
            {
                //从Resouces下加载
                t = Resources.Load<T>(resName);
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
                Debug.LogError($"找不到这样的资源文件!{resName}");
                return null;
            }
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
