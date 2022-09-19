using System;

public class Modifier
{
    private ModifierType type;

    public Action ModifierToRegister;
    public Action ModifierToRemove;

    public ModifierType Type => type;

    public Modifier(ModifierType type, Action modifierToRegister, Action modifierToRemove)
    {
        this.type = type;
        ModifierToRegister = modifierToRegister;
        ModifierToRemove = modifierToRemove;
    }
}

public interface IModifier
{
    public void RegisterModifier();
    public void RemoveModifier();
}

public enum ModifierType
{
    Modifier_Trinket_CurvedHorn,

    Modifier_Item_DamageMultiplierUp,
    Modifier_Item_TheBookOfBelial,
}
