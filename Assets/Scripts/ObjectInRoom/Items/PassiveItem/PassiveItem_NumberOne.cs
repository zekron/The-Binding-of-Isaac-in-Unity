using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_NumberOne : PassiveItem
{
    protected override void PassiveSkill()
    {
        gamePlayer.Tears += 1.5f;
        gamePlayer.Range -= 13;
    }
}
