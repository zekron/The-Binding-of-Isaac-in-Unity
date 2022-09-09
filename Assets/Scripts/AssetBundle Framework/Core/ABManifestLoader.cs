using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    public class ABManifestLoader
    {
        protected AssetBundle _AssetBundle;
        private Dictionary<string, string[]> _Dependences;
        private AssetBundleManifest _ABManifest;
        public ABManifestLoader(AssetBundle ab)
        {
            _AssetBundle = ab;
            _ABManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            _Dependences = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// AssetBundleManifest.GetAllDependencies(string assetBundleName)
        /// </summary>
        /// <param name="assetBundleName">Name of the asset bundle</param>
        /// <returns></returns>
        public string[] GetDependences(string assetBundleName)
        {
            string[] res;
            if (_Dependences.TryGetValue(assetBundleName, out res))
            {
                return res;
            }
            res = _Dependences[assetBundleName] = _ABManifest.GetAllDependencies(assetBundleName);
            //Debug.Log($"在这里获得所有的依赖关系: <color=green>{abName}</color> 的数量大概为: {res.Length} ");
            //if (res.Length > 0) Debug.Log($"\t<color=green>{abName}</color> depends on <color=#00ff5eff>{string.Join(", ", res)}</color>");
            return res;
        }
        public void Release()
        {
            this._AssetBundle.Unload(true);
            _ABManifest = null;
            _Dependences.Clear();
        }
    }
}
