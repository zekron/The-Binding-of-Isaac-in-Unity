using UnityEngine;

public class PassiveItem_RoidRage : PassiveItem
{
    [SerializeField] private HeadSpriteGroup roidRageHeadGroup;
    protected override void PassiveSkill()
    {
        gamePlayer.MoveSpeed += 0.6f;
        gamePlayer.SetHeadSpriteGroup(roidRageHeadGroup);
    }
}
