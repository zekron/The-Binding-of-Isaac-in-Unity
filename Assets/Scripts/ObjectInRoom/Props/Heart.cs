using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PickupObject
{
    [SerializeField] private HeartSO heartSO;

    private HeartSO.HeartType heartType;
    private HealthData heartWorth;

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

    public override bool CanPickUp()
    {
       return platform.SelfCollider.IsTrigger = !gamePlayer.IsFullHealth();
    }

    public override void OnPlayerCollect()
    {
        gamePlayer.GetHealing(heartWorth);

        platform.enabled = false;
        //gameObject.SetActive(false);
    }
}
