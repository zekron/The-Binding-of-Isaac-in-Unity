using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PickupObject
{
    [SerializeField] private HeartSO heartSO;

    private SpriteRenderer heartRenderer;
    private HeartSO.HeartType heartType;
    private int bombWorth;

    protected override void Awake()
    {
        base.Awake();

        heartRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        heartType = heartSO.GenerateType();
        heartRenderer.sprite = heartSO.HeartSprites[(int)heartType];
        bombWorth = BombSO.BombWorth[(int)heartType];
    }
}
