using System.Collections.Generic;

namespace AssetBundleFramework
{
    /// <summary>
    /// assetbundle ¿ò¼ÜµÄÅäÖÃÎÄ¼þ
    /// </summary>
    public class ABIni
    {
        public class Data
        {
            public string ResName;
            public string ABName;
            public Data(string resName, string abName)
            {
                this.ResName = resName;
                this.ABName = abName;
            }
        }
        public List<Data> Datas;
        public ABIni()
        {
            Datas = new List<Data>();
        }
        public void AddData(string resName, string abName)
        {
            Datas.Add(new Data(resName, abName));
        }

        public void Clear()
        {
            Datas.Clear();
        }
    }
}
