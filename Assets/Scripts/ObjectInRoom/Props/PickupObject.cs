using CustomPhysics2D;
using System;
using UnityEngine;

public abstract class PickupObject : RoomObject
{
    protected string clipNameOnPicked = "Pickup_OnPicked";
    protected Player gamePlayer;
    private Animation pickupAnimation;

    protected override void Awake()
    {
        collisionController = GetComponentInChildren<CustomCollisionController>();
        objectRenderer = GetComponentInChildren<SpriteRenderer>();

        pickupAnimation = GetComponent<Animation>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        collisionController.onCollisionEnter += Collect;
        collisionController.onTriggerEnter += Collect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        collisionController.onCollisionEnter -= Collect;
        collisionController.onTriggerEnter -= Collect;
    }

    public override void ResetObject()
    {
        gameObject.SetActive(true);
        collisionController.enabled = true;

        pickupAnimation.Play();
    }

    public virtual void Collect(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gamePlayer))
        {
            if (!CanPickUp())
            {
                OnPlayerCannotCollect(collisionInfo);
            }
            else
            {
                pickupAnimation.Play(clipNameOnPicked);

                OnPlayerCollect();
            }
        }
    }


    public void OnPickupAnimationFinished()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnPlayerCannotCollect(CollisionInfo2D collisionInfo) { }
    protected virtual bool CanPickUp() => true;
    protected abstract void OnPlayerCollect();
}
