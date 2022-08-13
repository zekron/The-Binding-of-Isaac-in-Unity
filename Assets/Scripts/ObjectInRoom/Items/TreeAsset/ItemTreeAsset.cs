using System.Collections.Generic;
using UnityEngine;

public abstract class ItemTreeAsset : ScriptableObject
{
    [SerializeField] protected List<ItemTreeElement> m_TreeElements;

    public abstract ItemTreeElement CreateProfile();

    public virtual ItemTreeElement GetProfileByID(int elementID)
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
