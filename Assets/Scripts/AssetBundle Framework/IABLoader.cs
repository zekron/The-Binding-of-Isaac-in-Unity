using UnityEngine;

namespace AssetBundleFramework
{
    public abstract class IABLoader
    {
        protected AssetBundle _AssetBundle;
        protected string _ABName;
        /// <summary>
        /// �Ƿ��ǳ�פLoader
        /// </summary>
        protected bool IsPersistent;
        public IABLoader(AssetBundle ab, string abName, bool isPersistent)
        {
            this._AssetBundle = ab;
            this._ABName = abName;
            this.IsPersistent = isPersistent;
        }
        /// <summary>
        /// �ͷ�AssetBundle����Դ����ͱ����õ���Դ�ڴ�
        /// </summary>
        public abstract void Release();
    }
}
