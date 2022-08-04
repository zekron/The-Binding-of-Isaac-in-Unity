using CustomPhysics2D;
using System;
using UnityEngine;

public abstract class PickupObject : RoomObject
{
    protected Player gamePlayer;
    private Animation pickupAnimation;

    protected override void Awake()
    {
        platform = GetComponentInChildren<CustomCollisionController>();
        objectRenderer = GetComponentInChildren<SpriteRenderer>();

        pickupAnimation = GetComponent<Animation>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        platform.onCollisionEnter += Collect;
        platform.onTriggerEnter += Collect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        platform.onCollisionEnter -= Collect;
        platform.onTriggerEnter -= Collect;
    }

    public override void ResetObject()
    {
        gameObject.SetActive(true);
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

    public void OnPickupAnimationFinished()
    {
        gameObject.SetActive(false);
    }

    public virtual bool CanPickUp() => true;
    public abstract void OnPlayerCollect();
}
