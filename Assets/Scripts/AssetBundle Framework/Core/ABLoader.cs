using System.Collections;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// 类的职责：负责AB资源的加载，具有缓存
    /// 注意：只需要一个Loader
    /// </summary>
    public class ABLoader : IABLoader
    {
        private Hashtable _ResCache;
        //只有一种Manifest文件
        public ABLoader(AssetBundle ab, string abName, bool isPersisent) : base(ab, abName, isPersisent)
        {
            //初始化缓存
            _ResCache = new Hashtable();
        }
        public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            if (_ResCache.ContainsKey(assetName))
            {
                Debug.Log($"找到缓存的Asset：{assetName}");
                return _ResCache[assetName] as T;
            }
            T t = _AssetBundle.LoadAsset<T>(assetName);
            _ResCache[assetName] = t;
            return t;
        }
        /// <summary>
        /// 从内存以及缓存中卸载指定的资源
        /// </summary>
        /// <param name="resName">资源名称</param>
        public void UnloadResource(string resName)
        {
            if (!_ResCache.ContainsKey(resName))
            {
                Debug.LogError($"未缓存这样的资源 -> {resName}");
            }
            else
            {
                //缓存中卸载
                _ResCache.Remove(resName);
            }
            //资源引用计数已经为0
            if (_ResCache.Count == 0)
                AssetBundleManager.Instance.Factory.ReleaseABLoader(_AssetBundle.name);
        }
        public override void Release()
        {
            if (IsPersistent)
            {
                Debug.LogWarning($"这个是常驻AB，不可以卸载:{_AssetBundle.name}");
                return;
            }
            _ResCache.Clear();
            _AssetBundle.Unload(true);
        }
    }
}
