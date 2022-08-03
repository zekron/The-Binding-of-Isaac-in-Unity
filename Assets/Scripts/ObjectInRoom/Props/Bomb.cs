using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PickupObject
{
    [SerializeField] private BombSO bombSO;

    private BombSO.BombType bombType;
    private int bombWorth;

    public void SetType(BombSO.BombType type)
    {
        bombType = type;
        objectRenderer.sprite = bombSO.BombSprites[(int)bombType];
        bombWorth = BombSO.BombWorth[(int)bombType];
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        SetType(bombSO.GenerateType());
    }

    public override void OnPlayerCollect()
    {
        gamePlayer.GetBomb(bombWorth);
        //else refresh context

        platform.enabled = false;
        //gameObject.SetActive(false);
    }
}
