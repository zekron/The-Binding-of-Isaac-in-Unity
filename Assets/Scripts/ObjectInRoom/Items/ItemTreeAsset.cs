using System.Collections.Generic;
using UnityEngine;

public abstract class ItemTreeAsset : ScriptableObject
{
    [SerializeField] protected List<CollectableItemData> m_TreeElements;

    public abstract CollectableItemData CreateProfile();

    public virtual CollectableItemData GetProfileByID(int elementID)
    {
        for (int i = 0; i < m_TreeElements.Count; i++)
        {
            if (m_TreeElements[i].ID == elementID)
                return m_TreeElements[i];
        }
#if UNITY_EDITOR
        CustomDebugger.ThrowException(string.Format("Failed to get profile by ID -> {0}", elementID));
        return CollectableItemData.Empty;
#else
        return CollectableItemData.Empty;
#endif
    }
}
