using CustomPhysics2D;
using UnityEngine;

/// <summary>
/// Item spawner, spawning different items with different RandomItemSO
/// </summary>
public class ItemPedestal : RoomObject
{
    /// <summary>
    /// Specific random items pool
    /// </summary>
    [SerializeField] private RandomItemSO randomObjectPool;
    private ItemObject item;

    protected override void OnEnable()
    {
        base.OnEnable();

        collisionController.onCollisionEnter += CollectItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        collisionController.onCollisionEnter -= CollectItem;
    }

    private void CollectItem(CollisionInfo2D collisionInfo)
    {
        //if (item == null) item = GetComponentInChildren<ItemObject>();

        if (collisionInfo.hitCollider.CompareTag("Player"))
        {
            var direction = (transform.position - collisionInfo.hitCollider.transform.position).normalized;
            (collisionController as CustomRigidbody2D).AddForce(direction);
        }

        if (item == null) return;

        item.Collect(collisionInfo);

        if (item is ActiveItem)
        {
            var activeItem = item as ActiveItem;
            if (activeItem.OldActiveItemData != null)
            {
                activeItem.OldActiveItemData.ItemPrefab.SetActive(false);
                item = ObjectPoolManager.Release(activeItem.OldActiveItemData.ItemPrefab,
                                                 transform.position, Quaternion.identity, transform).GetComponent<ItemObject>();
            }
            else item = null;
        }
        else
            item = null;
    }

    private void InstantiateRandomRoomObject()
    {
        randomObjectPool.InitializePrefabPool();
        item = ObjectPoolManager.Release(randomObjectPool.GenerateObject(),
                                         transform.position).GetComponent<ItemObject>();
        ChangeRendererOrder();
    }

    public override void ResetObject()
    {
        InstantiateRandomRoomObject();
    }
    public override void ChangeRendererOrder()
    {
        base.ChangeRendererOrder();

        item.ItemRenderer.sortingOrder = objectRenderer.sortingOrder + 1;
    }
}
