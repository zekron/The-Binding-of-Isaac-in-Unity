using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItem : ItemObject
{
    protected virtual void OnEnable()
    {
        if (itemData == null)
            itemData = ItemManager.Instance.GetCollectibleItemProfileByID(itemID);
    }
    protected override void OnPlayerCollect() 
    {
        CollectInCollection();
    }

    public void CollectInCollection()
    {
        if (true)
        {
            Debug.Log("CollectInCollection");
        }
    }
}
