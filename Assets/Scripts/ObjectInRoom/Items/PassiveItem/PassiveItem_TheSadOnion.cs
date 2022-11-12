using UnityEngine;

public class PassiveItem_TheSadOnion : PassiveItem
{
    [SerializeField] private HeadSpriteGroup sadOnionHeadGroup;
    protected override void PassiveSkill()
    {
        gamePlayer.Tears += 0.7f;
        gamePlayer.SetHeadSpriteGroup(sadOnionHeadGroup);
    }
}
