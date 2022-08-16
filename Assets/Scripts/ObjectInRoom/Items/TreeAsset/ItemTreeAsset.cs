using System.Collections.Generic;
using UnityEngine;

public abstract class ItemTreeAsset<T> : ScriptableObject where T : ItemTreeElement
{
    [SerializeField] protected List<T> m_TreeElements;

    public List<T> TreeElements => m_TreeElements;
    public T TreeRoot => m_TreeElements[0];

    public abstract T CreateProfile();

    public virtual T GetProfileByID(int elementID)
    {
        for (int i = 0; i < m_TreeElements.Count; i++)
        {
            if (m_TreeElements[i].ElementID == elementID)
                return m_TreeElements[i];
        }
#if UNITY_EDITOR
        CustomDebugger.ThrowException(string.Format("Failed to get profile by ID -> {0}", elementID));
        return null;
#else
        return null;
#endif
    }
}
