using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickupObject
{
    [SerializeField] private KeySO keySO;

    private SpriteRenderer keyRenderer;
    private KeySO.KeyType keyType;
    private int keyWorth;

    protected override void Awake()
    {
        base.Awake();

        keyRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        keyType = keySO.GenerateType();
        keyRenderer.sprite = keySO.KeySprites[(int)keyType];
        keyWorth = BombSO.BombWorth[(int)keyType];
    }
}
