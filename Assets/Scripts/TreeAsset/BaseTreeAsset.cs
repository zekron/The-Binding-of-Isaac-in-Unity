using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTreeAsset<T> : ScriptableObject where T : TreeElement
{
    [SerializeField] protected List<T> m_TreeElements;
    
    public List<T> TreeElements => m_TreeElements;
    public T TreeRoot => m_TreeElements[0];
    public int TreeCount => m_TreeElements.Count;

    public virtual T GetProfileByID(int elementID)
    {
        for (int i = 0; i < m_TreeElements.Count; i++)
        {
            if (m_TreeElements[i].ElementID == elementID)
                return m_TreeElements[i];
        }
#if UNITY_EDITOR
        Debug.LogError(string.Format("Failed to get profile by ID -> {0}", elementID));
        return null;
#else
        return null;
#endif
    }

    protected int GenerateUniqueID()
    {
        //FirstMissingPositive
        var nums = GetTreeElementIDs();
        for (int i = 0; i < nums.Length; i++)
        {
            //如果在指定的位置就不需要修改
            if (i + 1 == nums[i])
                continue;
            //如果不在则交换到数组对应下标的位置
            int x = nums[i];
            if (x >= 1 && x <= nums.Length && x != nums[x - 1])
            {
                Swap(nums, i, x - 1);
                i--;//抵消上面的i++，如果交换之后就不++；
            }
        }
        //最后在执行一遍循环，查看对应位置的元素是否正确，如果不正确直接返回
        for (int i = 0; i < nums.Length; i++)
        {
            if (i + 1 != nums[i])
                return i + 1;
        }
        return nums.Length + 1;
    }

    #region Miscs
    private int[] GetTreeElementIDs()
    {
        var IDArray = new int[m_TreeElements.Count];
        for (int i = 0; i < IDArray.Length; i++)
        {
            IDArray[i] = m_TreeElements[i].ElementID;
        }
        return IDArray;
    }
    private void Swap(int[] A, int i, int j)
    {
        if (i != j)
        {
            A[i] ^= A[j];
            A[j] ^= A[i];
            A[i] ^= A[j];
        }
    }
    #endregion

    public abstract T CreateProfile();
}
