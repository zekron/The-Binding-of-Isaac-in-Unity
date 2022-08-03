using CustomPhysics2D;
using System;
using UnityEngine;

public abstract class PickupObject : RoomObject
{
    protected Player gamePlayer;
    private Animation pickupAnimation;

    protected override void Awake()
    {
        base.Awake();

        pickupAnimation = GetComponentInParent<Animation>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        platform.onCollisionEnter += Collect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        platform.onCollisionEnter -= Collect;
    }

    public override void ResetObject()
    {
        //gameObject.SetActive(true);
        platform.enabled = true;

        pickupAnimation.Play();
    }

    public virtual void Collect(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gamePlayer))
        {
            if (!CanPickUp()) return;

            pickupAnimation.Play("Pickup_OnPicked");

            OnPlayerCollect();
        }
    }

    public virtual bool CanPickUp() => true;
    public abstract void OnPlayerCollect();
}
