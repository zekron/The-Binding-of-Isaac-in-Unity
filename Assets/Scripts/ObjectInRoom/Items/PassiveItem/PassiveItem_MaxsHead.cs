using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_MaxsHead : PassiveItem
{
    protected override void PassiveSkill()
    {
        if (!gamePlayer.CheckPassiveItem(CollectibleItemName.Blood_Of_The_Martyr, CollectibleItemName.Magic_Mushroom))
            gamePlayer.DamageMultiplier *= 1.5f;

        gamePlayer.BaseDamageUps += 1;
    }
}
