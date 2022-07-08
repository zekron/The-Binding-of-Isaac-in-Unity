using CustomPhysics2D;
using UnityEngine;

public class PickupObject : RoomObject
{
    private Animation pickupAnimation;

    protected override void Awake()
    {
        base.Awake();

        pickupAnimation = GetComponentInParent<Animation>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        platform.onTriggerEnter += Collect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        platform.onTriggerEnter -= Collect;
    }

    public override void ResetObject()
    {
        pickupAnimation.Play("Pickup_Idle");
    }

    public virtual void Collect(CollisionInfo2D collisionInfo)
    {
        pickupAnimation.Play("Pickup_OnPicked");
    }
}
