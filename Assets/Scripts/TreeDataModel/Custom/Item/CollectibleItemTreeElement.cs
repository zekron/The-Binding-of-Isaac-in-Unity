using UnityEngine;

[System.Serializable]
public class CollectibleItemTreeElement : ItemTreeElement
{
    public new readonly static int TreeViewColumnsLength = 7;
    public Sprite CollectionSprite;

    public CollectibleItemTreeElement(int id, string name, int depth, string description, Sprite itemSprite, Sprite collectionSprite) : base(id, name, depth, description, itemSprite)
    {
        CollectionSprite = collectionSprite;
    }
}
