using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketItem_CurvedHorn : TrinketItem, IModifier
{
    private ModifierType itemModifierType = ModifierType.Modifier_Trinket_CurvedHorn;

    protected override void OnEnable()
    {
        base.OnEnable();

        gamePlayer.RemoveSpecificEffect(itemModifierType);
    }

    protected override void SpecificEffect()
    {
        gamePlayer.AddSpecificEffect(new Modifier(itemModifierType, RegisterModifier, RemoveModifier));
    }

    public void RegisterModifier()
    {
        gamePlayer.FlatDamageUps += 2;
    }

    public void RemoveModifier()
    {
        gamePlayer.FlatDamageUps -= 2;
    }
}
