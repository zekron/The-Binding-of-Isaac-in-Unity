using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickupObject
{
    [SerializeField] private KeySO keySO;

    private KeySO.KeyType keyType;
    private int keyWorth;

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        keyType = keySO.GenerateType();
        objectRenderer.sprite = keySO.KeySprites[(int)keyType];
        keyWorth = KeySO.KeyWorths[(int)keyType];
    }

    public override void OnPlayerCollect()
    {
        gamePlayer.GetKey(keyWorth);

    }
}
