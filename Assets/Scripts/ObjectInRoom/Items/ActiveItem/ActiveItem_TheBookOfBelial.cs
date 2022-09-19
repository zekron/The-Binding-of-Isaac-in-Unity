using System;
using UnityEngine;

public class ActiveItem_TheBookOfBelial : ActiveItem
{
    [Header("Specific Fields")]
    [SerializeField] private float modifierBaseDamageUp = 2;
    [SerializeField] private float modifierDamageMultiplier = 1.5f;
    [SerializeField] private TwoVector3EventChannelSO onEnterRoomEvent;
    [SerializeField] private HeadSpriteGroup headSpriteGroup;

    private ModifierType baseModifierType = ModifierType.Modifier_Item_DamageMultiplierUp;
    private ModifierType itemModifierType = ModifierType.Modifier_Item_TheBookOfBelial;
    private Modifier baseModifier;
    private Modifier itemModifier;

    protected override void Awake()
    {
        base.Awake();

        baseModifier = new Modifier(baseModifierType, RegisterBaseModifier, RemoveBaseModifier);
        itemModifier = new Modifier(itemModifierType, RegisterDamageMultiUp, RemoveDamageMultiUp);
    }

    private void RegisterDamageMultiUp()
    {
        if (gamePlayer.CheckPassiveItem(CollectibleItemName.Blood_Of_The_Martyr) &&
            !gamePlayer.CheckPassiveItem(CollectibleItemName.Magic_Mushroom, CollectibleItemName.Maxs_Head))
            gamePlayer.DamageMultiplier += modifierDamageMultiplier;
    }
    private void RegisterBaseModifier()
    {
        gamePlayer.SetHeadSpriteGroup(headSpriteGroup);

        gamePlayer.BaseDamageUps += modifierBaseDamageUp;
    }
    private void RemoveDamageMultiUp()
    {
        if (gamePlayer.CheckPassiveItem(CollectibleItemName.Blood_Of_The_Martyr) &&
            !gamePlayer.CheckPassiveItem(CollectibleItemName.Magic_Mushroom, CollectibleItemName.Maxs_Head))
            gamePlayer.DamageMultiplier -= modifierDamageMultiplier;
    }
    private void RemoveBaseModifier()
    {
        gamePlayer.RevertHeadSpriteGroup();

        gamePlayer.BaseDamageUps -= modifierBaseDamageUp;
    }

    private void RemoveModifier(Vector3 arg0, Vector3 arg1)
    {
        gamePlayer.RemoveSpecificEffect(baseModifierType);
        gamePlayer.RemoveSpecificEffect(itemModifierType);

        onEnterRoomEvent.OnEventRaised -= RemoveModifier;
    }

    protected override void SpecificActiveSkill()
    {
        gamePlayer.AddSpecificEffect(baseModifier);
        gamePlayer.AddSpecificEffect(itemModifier);

        onEnterRoomEvent.OnEventRaised += RemoveModifier;
    }

    protected override void RegisterSpecificPassiveModifier()
    {
        //gamePlayer.DevilRoomRatio += 25;
    }

    protected override void RemoveSpecificPassiveModifier()
    {
        //gamePlayer.DevilRoomRatio -= 25;
    }
}
