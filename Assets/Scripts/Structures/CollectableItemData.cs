using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollectableItemData
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite ItemSprite;
    public Sprite CollectionSprite;

    public CollectableItemData(int iD, string name, string description, Sprite itemSprite, Sprite collectionSprite)
    {
        ID = iD;
        Name = name;
        Description = description;
        ItemSprite = itemSprite;
        CollectionSprite = collectionSprite;
    }
    
    public static CollectableItemData Empty => new CollectableItemData(-1, null, null, null, null);
}
