using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketItemTreeAsset : ItemTreeAsset
{
    public TrinketItemTreeAsset()
    {
        m_TreeElements = new List<CollectableItemData>()
        {
            new CollectableItemData(-1, "Root", "Null", null, null),
        };
        CreateProfile();
    }
    public override CollectableItemData CreateProfile()
    {
        CollectableItemData result = new CollectableItemData(1, "Name here", "Description here", null, null);
        m_TreeElements.Add(result);
        return result;
    }
}
