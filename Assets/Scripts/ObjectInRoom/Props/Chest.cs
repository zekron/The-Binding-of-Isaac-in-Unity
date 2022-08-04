using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : RoomObject
{
    [SerializeField] private ChestSO chestSO;

    private ChestSO.ChestType chestType;
    protected Player gamePlayer;
    private CustomFrameAnimation openAnimation;

    private bool isOpen = false;

    public void SetType(ChestSO.ChestType type)
    {
        chestType = type;

        objectRenderer.sprite = chestSO.ChestSprites[(int)type];
        openAnimation.ChangeClip(chestSO.OpenChestClips[(int)type]);
    }

    protected override void Awake()
    {
        base.Awake();

        openAnimation = GetComponent<CustomFrameAnimation>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        platform.onCollisionEnter += Open;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        platform.onCollisionEnter -= Open;
    }

    private void Open(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gamePlayer))
        {
            if (isOpen) return;

            if (chestType == ChestSO.ChestType.Locked)
            {
                if (gamePlayer.KeyCount < 1) return;

                if (!gamePlayer.GetGoldenKey)
                    gamePlayer.UseKey(1);
            }
            openAnimation.Play();
            chestSO.SpawnReward(chestType, transform.position);
            isOpen = true;
        }
    }

    public override void ResetObject()
    {
        openAnimation.ResetAnimation();
        isOpen = false;

        SetType(chestSO.GenerateType());
    }
}
