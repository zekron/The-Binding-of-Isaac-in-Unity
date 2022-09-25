using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem_Poop : ActiveItem
{
    [Header("Specific Item")]
    [SerializeField] private GameObject poopPrefab;

    private CustomCollisionController collisionController;

    protected override void SpecificActiveSkill()
    {
        collisionController = ObjectPoolManager.Release(poopPrefab, gamePlayer.transform.position).GetComponent<CustomCollisionController>();

        //collisionController.SelfCollider.IsTrigger = true;
        collisionController.onCollisionEnter += OnPlayerEnter;
    }

    private void OnPlayerEnter(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out Player player))
        {
            collisionController.SelfCollider.IsTrigger = true;

            collisionController.onCollisionEnter -= OnPlayerEnter;
            collisionController.onTriggerExit += OnPlayerExit;
        }
    }

    private void OnPlayerExit(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out Player player))
        {
            collisionController.SelfCollider.IsTrigger = false;

            collisionController.onTriggerExit -= OnPlayerExit;
        }

    }
}
