using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PickupObject
{
    [SerializeField] private BombSO bombSO;

    private SpriteRenderer bombRenderer;
    private BombSO.BombType bombType;
    private int bombWorth;

    protected override void Awake()
    {
        base.Awake();

        bombRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        bombType = bombSO.GenerateType();
        bombRenderer.sprite = bombSO.BombSprites[(int)bombType];
        bombWorth = BombSO.BombWorth[(int)bombType];
    }
}
