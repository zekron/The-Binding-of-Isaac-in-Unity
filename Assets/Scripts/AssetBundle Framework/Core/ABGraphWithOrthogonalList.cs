using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// AB框架的管理容器
    /// 是用来管理AB包依赖的，所以不可能出现循环引用包关系。
    /// </summary>
    public class ABGraphWithOrthogonalList
    {
        public class VexNode
        {
            public ABLoader Loader;
            public ArcNode FirstIn; //指向第一个入度
            public ArcNode FirstOut;//指向第一个出度
            public VexNode(ABLoader loader, ArcNode firstin = null, ArcNode firstout = null)
            {
                this.Loader = loader;
                this.FirstIn = firstin;
                this.FirstOut = firstout;
            }
        }
        public class ArcNode
        {
            public string HeadABName; //弧头AB包的名称,出度根据弧头获得值
            public string TailABName; //弧尾AB包的名称 
            public ArcNode HLink;     //指向同弧头的弧
            public ArcNode TLink;     //指向同弧尾的弧 

            public ArcNode(string tail, string head, ArcNode hlink = null, ArcNode tlink = null)
            {
                this.HeadABName = head;
                this.TailABName = tail;
                this.HLink = hlink;
                this.TLink = tlink;
            }
        }
        Dictionary<string, VexNode> _DicVexs;
        public ABGraphWithOrthogonalList()
        {
            _DicVexs = new Dictionary<string, VexNode>();
        }
        /// <summary>
        /// 查询是否存在这样一个结点
        /// </summary>
        /// <param name="abName"></param>
        public bool ContainVexNode(string abName)
        {
            return _DicVexs.ContainsKey(abName);
        }
        /// <summary>
        /// 获得一个结点
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public VexNode GetVexNode(string abName)
        {
            if (ContainVexNode(abName))
                return _DicVexs[abName];
            Debug.LogError("没有缓存这样的节点，无法获得");
            return null;
        }
        /// <summary>
        /// 给图中添加一个结点
        /// </summary>
        public void AddVexNode(string abName, VexNode vex)
        {
            if (_DicVexs.ContainsKey(abName))
            {
                Debug.LogWarning($"内存中已经存在 {abName} 还未释放！AddVexNode");
                return;
            }
            _DicVexs.Add(abName, vex);
        }
        public void AddVexNode(string abName, ABLoader loader)
        {
            if (_DicVexs.ContainsKey(abName))
            {
                Debug.LogWarning($"内存中已经存在 {abName} 还未释放！AddVexNode");
                return;
            }
            _DicVexs.Add(abName, new VexNode(loader));
        }
        /// <summary>
        /// 删除一个对应的AssetBundle结点
        /// 但是要ASsetBundle的入度为0才可以删除，一个已经被引用了的AssetBundle不能删除
        /// </summary>
        /// <param name="abName"></param>
        public bool RemoveVexNode(string abName)
        {
            if (!_DicVexs.ContainsKey(abName))
            {
                Debug.LogWarning($"要删除的ab包 {abName} 还未加载进内存中！RemoveVexNode");
                return false;
            }
            //先删除与这个顶点有关的所有弧
            VexNode v = _DicVexs[abName];
            if (v.FirstIn != null)
            {
                Debug.LogError($"{abName} 正在被 {v.FirstIn.TailABName} 等资源包引用中，所以不能卸载");
                return false;
            }
            for (int i = 0; i < _DicVexs.Keys.Count; i++)
            {
                string anotherAbName = _DicVexs.Keys.ElementAt(i);
                Debug.Log($"RemoveVexNode测试：{anotherAbName}");
                if (anotherAbName == abName)
                    continue;
                else
                {
                    RemoveArc(abName, anotherAbName); //删除出弧
                }
            }
            _DicVexs.Remove(abName);
            Debug.Log("删除完成");
            return true;
        }
        /// <summary>
        /// 要确保对应的ab包都已经加载!!!
        /// 添加图的边关系
        /// ab包1->ab包2(ab包1依赖于ab包2)
        /// </summary>
        /// <param name="abName1">ab包1</param>
        /// <param name="abName2">ab包2</param>
        public void InsertArc(string abName1, string abName2)
        {
            ArcNode an = new ArcNode(abName1, abName2);
            VexNode v1 = _DicVexs[abName1];
            VexNode v2 = _DicVexs[abName2];
            if (v1 == null)
            {
                Debug.Log($"{abName1} 包还未加载！ InsertEdge");
            }
            if (v2 == null)
            {
                Debug.Log($"{abName2} 包还未加载！ InsertEdge");
            }
            //构建出度
            an.TLink = v1.FirstOut;
            v1.FirstOut = an;
            //构建入度
            an.HLink = v2.FirstIn;
            v2.FirstIn = an;
        }

        /// <summary>
        /// 删除ab1->ab2的弧
        /// </summary>
        /// <param name="abName1">ab包1名称</param>
        /// <param name="abName2">ab包2名称</param>
        public void RemoveArc(string abName1, string abName2)
        {
            VexNode v1 = _DicVexs[abName1];
            VexNode v2 = _DicVexs[abName2];
            //删除v1的出度
            if (v1.FirstOut == null)
            {
                Debug.LogWarning($"没有由 {abName1} -> {abName2} 这条边！ RemoveArc");
                return;
            }
            ArcNode curOut = v1.FirstOut;
            if (curOut.HeadABName == abName2)
            {
                Debug.Log($"找到需要移除的边！ {curOut.TailABName} -> {curOut.HeadABName}! RemoveArc");
                v1.FirstOut = curOut.TLink;
            }
            else
            {
                while (curOut.TLink != null && curOut.TLink.HeadABName != abName2)
                {
                    curOut = curOut.TLink;
                }
                if (curOut.TLink == null)
                {
                    Debug.LogWarning($"没有由 {abName1} -> {abName2} 这条边！ RemoveArc");
                    return;
                }
                Debug.Log($"找到需要移除的边！ {curOut.TailABName} -> {curOut.HeadABName}！ RemoveArc");
                curOut.TLink = curOut.TLink.TLink;
            }
            //删除v2的入度
            ArcNode curIn = v2.FirstIn;
            if (curIn.TailABName == abName1)
            {
                v2.FirstIn = curIn.HLink;
            }
            else
            {
                while (curIn.HLink != null && curIn.HLink.TailABName != abName1)
                {
                    curIn = curIn.HLink;
                }
                curIn.HLink = curIn.HLink.HLink;
            }
        }
        /// <summary>
        /// 完全释放资源
        /// </summary>
        public void ClearAll()
        {
            foreach (var kv in _DicVexs)
            {
                kv.Value.Loader.Release();
            }
            _DicVexs.Clear();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //打印出所有的出度和入度
            foreach (KeyValuePair<string, VexNode> kv in _DicVexs)
            {
                sb.Append($"{kv.Key}的出度为：");
                ArcNode curOUt = kv.Value.FirstOut;
                while (curOUt != null)
                {
                    sb.Append($"{curOUt.HeadABName}，");
                    curOUt = curOUt.TLink;
                }
                sb.Append("NULL");
                sb.Append("入度为:");
                ArcNode curIn = kv.Value.FirstIn;
                while (curIn != null)
                {
                    sb.Append($"{curIn.TailABName}，");
                    curIn = curIn.HLink;
                }
                sb.AppendLine("NULL");
            }
            return sb.ToString();
        }
    }
}

