using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem_TheInnerEye : PassiveItem
{
    [SerializeField] private HeadSpriteGroup innerEyeHeadGroup;
    [SerializeField] private GameObject innerEyeMuzzle;
    protected override void PassiveSkill()
    {
        gamePlayer.TearMultiplier += 2;
        gamePlayer.TearAddition += 3;
        gamePlayer.SetHeadSpriteGroup(innerEyeHeadGroup);
        gamePlayer.ChangeMuzzle(innerEyeMuzzle);

        Debug.Log(gamePlayer.TearDelay);
    }
}
