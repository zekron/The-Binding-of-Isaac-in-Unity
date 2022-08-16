using UnityEngine;

public class CollectibleItemTreeElement : ItemTreeElement
{
    public Sprite CollectionSprite;

    public CollectibleItemTreeElement(int id, string name, int depth, string description, Sprite itemSprite, Sprite collectionSprite) : base(id, name, depth, description, itemSprite)
    {
        CollectionSprite = collectionSprite;
    }
}
