using UnityEngine;

public class CollectableItemTreeElement : ItemTreeElement
{
    public Sprite CollectionSprite;

    public CollectableItemTreeElement(int id, string name, int depth, string description, Sprite itemSprite, Sprite collectionSprite) : base(id, name, depth, description, itemSprite)
    {
        CollectionSprite = collectionSprite;
    }
}
