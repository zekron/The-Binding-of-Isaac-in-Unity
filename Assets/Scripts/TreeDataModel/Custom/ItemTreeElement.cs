using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTreeElement : TreeElement
{
    public string Description;
    public Sprite ItemSprite;

    public ItemTreeElement(int id, string name, int depth, string description, Sprite itemSprite) : base(name, depth, id)
    {
        Description = description;
        ItemSprite = itemSprite;
    }
}
