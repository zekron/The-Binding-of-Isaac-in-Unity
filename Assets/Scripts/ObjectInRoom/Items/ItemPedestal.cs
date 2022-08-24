using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPedestal : RoomObject
{
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
        if (item == null) item = GetComponentInChildren<ItemObject>();

        item.Collect(collisionInfo);


        if (item is ActiveItem)
        {
            var activeItem = item as ActiveItem;
            if (activeItem.OldActiveItemData != null)
                item = ObjectPoolManager.Release(activeItem.OldActiveItemData.ItemPrefab,
                                            transform.position,
                                            Quaternion.identity,
                                            transform).GetComponent<ItemObject>();
        }
    }

    private void InstantiateRandomRoomObject()
    {
        randomObjectPool.InitializePrefab();
        item = ObjectPoolManager.Release(randomObjectPool.GenerateObject(),
                                            transform.position,
                                            Quaternion.identity,
                                            transform).GetComponent<ItemObject>();
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
