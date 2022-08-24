using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Item SO", menuName = "Scriptable Object/Random Object/Random Item")]
public class RandomItemSO : RandomObjectBaseSO<RandomItemData>
{
    public void InitializePrefab()
    {
        for (int i = 0; i < RandomObjectArray.Length; i++)
        {
            RandomObjectArray[i].RandomObject = ItemManager.Instance.GetCollectibleItemProfileByID((int)RandomObjectArray[i].itemName).ItemPrefab;
        }
    }
}
