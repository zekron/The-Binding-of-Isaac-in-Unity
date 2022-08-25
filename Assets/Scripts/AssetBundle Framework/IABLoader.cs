using UnityEngine;

namespace AssetBundleFramework
{
    public abstract class IABLoader
    {
        protected AssetBundle _AssetBundle;
        protected string _ABName;
        /// <summary>
        /// 是否是常驻Loader
        /// </summary>
        protected bool IsPersistent;
        public IABLoader(AssetBundle ab, string abName, bool isPersistent)
        {
            this._AssetBundle = ab;
            this._ABName = abName;
            this.IsPersistent = isPersistent;
        }
        /// <summary>
        /// 释放AssetBundle和资源缓存和被引用的资源内存
        /// </summary>
        public abstract void Release();
    }
}
