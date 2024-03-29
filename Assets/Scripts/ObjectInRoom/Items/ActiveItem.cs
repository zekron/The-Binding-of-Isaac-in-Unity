using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveItem : CollectableItem
{
    public const int MAX_NUMBER_OF_CHARGES = 6;
    [SerializeField] protected int numberOfCharges = 6;
    [SerializeField] private ActiveItemEventChannelSO onActiveItemChanged;
    [SerializeField] private VoidEventChannelSO onClearRoomEvent;

    protected int currentCharges;
    private CollectibleItemTreeElement oldActiveItemData = null;

    public CollectibleItemTreeElement OldActiveItemData => oldActiveItemData;

    protected virtual void Awake()
    {
        currentCharges = numberOfCharges;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        onClearRoomEvent.OnEventRaised += ChargeItem;
    }
    protected virtual void OnDisable()
    {
        onClearRoomEvent.OnEventRaised -= ChargeItem;

        RemoveSpecificPassiveModifier();
    }

    private void ChargeItem()
    {
        currentCharges++;
    }

    protected override void OnPlayerCollect()
    {
        base.OnPlayerCollect();

        oldActiveItemData = gamePlayer.GetActiveItem(itemData as CollectibleItemTreeElement, ActiveSkill);
        onActiveItemChanged.RaiseEvent(itemData.ItemSprite,
                                       Mathf.RoundToInt((float)currentCharges / numberOfCharges * StaticData.MAX_NUMBER_OF_CHARGES));

        RegisterSpecificPassiveModifier();
    }

    /// <summary>
    /// virtual, execute before override
    /// </summary>
    private void ActiveSkill()
    {
        if (currentCharges != numberOfCharges) return;

        currentCharges = 0;
        onActiveItemChanged.RaiseEvent(itemData.ItemSprite,
                                       Mathf.RoundToInt((float)currentCharges / numberOfCharges * StaticData.MAX_NUMBER_OF_CHARGES));

        SpecificActiveSkill();
    }

    /// <summary>
    /// Execute when collect. No need.
    /// </summary>
    protected virtual void RegisterSpecificPassiveModifier() { }
    /// <summary>
    /// Execute when OnDisable(). Override if have passive modifier.
    /// </summary>
    protected virtual void RemoveSpecificPassiveModifier() { }
    protected abstract void SpecificActiveSkill();
}
