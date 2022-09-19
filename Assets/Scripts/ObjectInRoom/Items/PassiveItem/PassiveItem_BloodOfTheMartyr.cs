using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_BloodOfTheMartyr : PassiveItem
{
    protected override void PassiveSkill()
    {
        //if (!gamePlayer.CheckPassiveItem(CollectibleItemName.Magic_Mushroom, CollectibleItemName.Maxs_Head))
        //    gamePlayer.DamageMultiplier += 1.5f;

        gamePlayer.BaseDamageUps += 1;
    }
}
