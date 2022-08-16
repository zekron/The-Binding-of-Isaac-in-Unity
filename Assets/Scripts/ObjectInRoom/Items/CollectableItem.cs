using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItem : RoomObject
{
    private CollectibleItemTreeElement temData;

    protected Player gamePlayer;
    private Animation pickupAnimation;

    protected override void Awake()
    {
        base.Awake();

        pickupAnimation = GetComponent<Animation>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void ResetObject()
    {
        objectRenderer.sprite = temData.ItemSprite;
    }

    private void Collect(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gamePlayer))
        {

            pickupAnimation.Play("Pickup_OnPicked");

            OnPlayerCollect();
        }
    }
    public abstract void OnPlayerCollect();

    public void CollectInCollection()
    {
        if (true)
        {
            Debug.Log("CollectInCollection");
        }
    }
}
