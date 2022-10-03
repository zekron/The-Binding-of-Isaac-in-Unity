using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PickupObject
{
    [SerializeField] private HeartSO heartSO;

    private HeartSO.HeartType heartType;
    private HealthData heartWorth;

    protected override void Awake()
    {
        base.Awake();

        collisionController = GetComponent<CustomCollisionController>();
        clipNameOnPicked = "Pickup_Heart_OnPicked";
    }

    public void SetType(HeartSO.HeartType type)
    {
        heartType = type;
        objectRenderer.sprite = heartSO.HeartSprites[(int)heartType];
        heartWorth = HeartSO.HeartWorths[(int)heartType];
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        SetType(heartSO.GenerateType());
    }

    protected override bool CanPickUp()
    {
        return collisionController.SelfCollider.IsTrigger = !gamePlayer.IsFullHealth();
    }

    protected override void OnPlayerCannotCollect(CollisionInfo2D collisionInfo)
    {
        var direction = (transform.position - collisionInfo.hitCollider.transform.position).normalized;
        (collisionController as CustomRigidbody2D).AddForce(direction * 1.5f);
    }
    protected override void OnPlayerCollect()
    {
        gamePlayer.GetHealing(heartWorth);

        collisionController.enabled = false;
        //gameObject.SetActive(false);
    }
}
