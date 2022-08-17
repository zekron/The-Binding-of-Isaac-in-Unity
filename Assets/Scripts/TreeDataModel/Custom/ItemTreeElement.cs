using UnityEngine;

[System.Serializable]
public class ItemTreeElement : TreeElement
{
    public readonly static int TreeViewColumnsLength = 5;
    public string ItemQuote;
    public string ItemDescription;
    public Sprite ItemSprite;
    public ItemTreeElement(int id, string name, int depth, string description, Sprite itemSprite) : base(name, depth, id)
    {
        ItemDescription = description;
        ItemSprite = itemSprite;
    }
}
