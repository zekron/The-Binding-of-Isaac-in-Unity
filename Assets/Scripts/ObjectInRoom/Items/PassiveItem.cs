using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : CollectableItem
{
    protected override void OnPlayerCollect()
    {
        base.OnPlayerCollect();

        gamePlayer.GetPassiveItem(itemData);

        PassiveSkill();
    }

    protected abstract void PassiveSkill();
}
