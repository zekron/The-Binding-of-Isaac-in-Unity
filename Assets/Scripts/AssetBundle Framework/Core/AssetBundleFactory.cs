using System.IO;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// 工厂设计模式
    /// </summary>
    public class AssetBundleFactory
    {
        public ABGraphWithOrthogonalList _Graph;
        /// <summary>
        /// Main AssetBundle Manifest, 用于检索所有ab包的依赖关系
        /// 会放在游戏结束
        /// </summary>
        private ABManifestLoader _ABManifestLoader;

        public AssetBundleFactory()
        {
            //在PersistentDataPath里面看看有没有对应的AssetBundle,配置文件信息
            AssetBundle ab;
            string manifestPath = ABDefine.GetABPackPath();
            //Debug.Log(manifestPath);
            ab = AssetBundle.LoadFromFile(manifestPath);
            var abManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            //初始化Manifest
            _ABManifestLoader = new ABManifestLoader(ab);
            string[] abs = abManifest.GetAllAssetBundles();
            foreach (string subAssetBundle in abs)
            {
                //AssetBundle.LoadFromFile(Path);
                _ABManifestLoader.GetDependences(subAssetBundle);
            }
            //初始化图
            _Graph = new ABGraphWithOrthogonalList();
        }

        /// <summary>
        /// 无依赖AssetBundle包的加载方式
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private ABLoader GetSingleABLoader(string abName, bool isPersistent)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(ABDefine.GetABPackPath(abName));
            if (ab == null)
            {
                Debug.LogError("没有可以加载的对应的AssetBundle，名称为：" + abName);
                return null;
            }
            ABLoader loader = new ABLoader(ab, abName, isPersistent); //不加缓存
            return loader;
        }

        /// <summary>
        /// 对外获得Assetbundle的方法，可能有依赖
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public ABLoader GetABLoader(string abName, bool isPersistent = false)
        {
            //查看缓存中是否拥有
            if (_Graph.ContainVexNode(abName))
            {
                return _Graph.GetVexNode(abName).Loader;
            }
            //先加载abName的依赖包
            string[] dependences = _ABManifestLoader.GetDependences(abName);
            for (int i = 0; i < dependences.Length; i++)
            {
                //Debug.Log($"{abName} 的依赖 -> {dependences[i]}");
                GetABLoader(dependences[i]);
            }
            //加载abName Loader
            ABLoader loader = GetSingleABLoader(abName, isPersistent);
            _Graph.AddVexNode(abName, loader);
            //构建弧的关系
            for (int i = 0; i < dependences.Length; i++)
            {
                _Graph.InsertArc(abName, dependences[i]); //出度关系
            }
            return loader;
        }

        /// <summary>
        /// 卸载某一个AssetBundle，是否卸载完全
        /// </summary>
        /// <param name="abName"></param>
        public void ReleaseABLoader(string abName)
        {
            if (!_Graph.ContainVexNode(abName))
            {
                Debug.LogWarning($"并未加载这样的AssetBundle{abName}，无法释放其资源");
                return;
            }
            ABLoader loader = _Graph.GetVexNode(abName).Loader;
            if (_Graph.RemoveVexNode(abName))
            {
                loader.Release();
                //Resources 卸载
                Resources.UnloadUnusedAssets();
            }
        }
        /// <summary>
        /// 卸载所有的AssetBundle，完全卸载
        /// </summary>
        public void ReleaseAllABLoader()
        {
            _Graph.ClearAll();
            //Resources 卸载
            Resources.UnloadUnusedAssets();
        }
    }
}
