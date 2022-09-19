using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_TheSadOnion : PassiveItem
{
    protected override void PassiveSkill()
    {
        gamePlayer.Tears += 0.7f;
    }
}
