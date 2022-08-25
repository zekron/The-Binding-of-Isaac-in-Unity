﻿using System.Collections.Generic;
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

        public string[] GetDependences(string abName)
        {
            string[] res;
            if (_Dependences.TryGetValue(abName, out res))
            {
                return res;
            }
            res = _Dependences[abName] = _ABManifest.GetAllDependencies(abName);
            Debug.Log($"在这里获得所有的依赖关系:{abName}的数量大概为:{res.Length}");
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
