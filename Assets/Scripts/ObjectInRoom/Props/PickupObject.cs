using CustomPhysics2D;
using System;
using UnityEngine;

public abstract class PickupObject : RoomObject
{
    protected string clipNameOnPicked = "Pickup_OnPicked";

    protected CustomRigidbody2D customRigidbody;
    protected Player gamePlayer;

    private PickupObject pickup;
    private Animation pickupAnimation;

    protected override void Awake()
    {
        collisionController = customRigidbody = GetComponent<CustomRigidbody2D>();
        objectRenderer = GetComponentInChildren<SpriteRenderer>();

        pickupAnimation = GetComponent<Animation>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        collisionController.onCollisionEnter += OnCustomCollisionEnter;
        //collisionController.onTriggerEnter += OnCustomCollisionEnter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        collisionController.onCollisionEnter -= OnCustomCollisionEnter;
        //collisionController.onTriggerEnter -= OnCustomCollisionEnter;
    }

    public override void ResetObject()
    {
        gameObject.SetActive(true);
        collisionController.enabled = true;

        pickupAnimation.Play();
    }

    public virtual void OnCustomCollisionEnter(CollisionInfo2D collisionInfo)
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
                //collisionController.SelfCollider.IsTrigger = true;

                OnPlayerCollect();
            }
        }

        if (collisionInfo.hitCollider.TryGetComponent(out pickup))
        {
            var direction = (transform.position - pickup.transform.position).normalized;
            var velocity = customRigidbody.velocity * 0.5f;
            pickup.customRigidbody.AddForce(direction * velocity);
        }
    }


    public void OnPickupAnimationFinished()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnPlayerCannotCollect(CollisionInfo2D collisionInfo) { }
    protected virtual bool CanPickUp() => collisionController.SelfCollider.IsTrigger = true;
    protected abstract void OnPlayerCollect();
}
