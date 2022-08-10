using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Scriptable Object/Items")]
public class Item : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite itemSprite;
}
