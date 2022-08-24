using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketItem : ItemObject
{
    protected override void OnPlayerCollect()
    {
        gamePlayer.GetTrinket(itemData.ElementID);
    }
}
