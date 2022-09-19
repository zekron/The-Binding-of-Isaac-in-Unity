public abstract class TrinketItem : ItemObject
{
    protected virtual void OnEnable()
    {
        if (itemData == null)
            itemData = ItemManager.Instance.GetTrinketItemProfileByID(itemID);
    }

    protected override void OnPlayerCollect()
    {
        gamePlayer.GetTrinket(itemData);

        SpecificEffect();
    }

    protected abstract void SpecificEffect();
}
