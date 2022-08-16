using UnityEngine;

[System.Serializable]
public class ItemTreeElement : TreeElement
{
    public static readonly int TreeViewColumnsLength = 4;
    public string Description;
    public Sprite ItemSprite;

    public ItemTreeElement(int id, string name, int depth, string description, Sprite itemSprite) : base(name, depth, id)
    {
        Description = description;
        ItemSprite = itemSprite;
    }
}
