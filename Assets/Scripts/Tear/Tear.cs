using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    private CustomFrameAnimation frameAnimation;
    private CustomRigidbody2D tearRigidbody2D;
    private IHealth healthItem;

    public Vector2 MoveVelocity { set => tearRigidbody2D.velocity = value; }
    private void Awake()
    {
        frameAnimation = GetComponent<CustomFrameAnimation>();
        tearRigidbody2D = GetComponent<CustomRigidbody2D>();
    }

    private void OnEnable()
    {
        tearRigidbody2D.onCollisionEnter += HitCollision;
    }
    private void OnDisable()
    {
        tearRigidbody2D.onCollisionEnter -= HitCollision;
        frameAnimation.ResetAnimation();
        tearRigidbody2D.SelfCollider.IsTrigger = false;
    }

    private void HitCollision(CollisionInfo2D collisionInfo)
    {
        if(collisionInfo.hitCollider.TryGetComponent(out healthItem))
        {
            Debug.Log(collisionInfo.hitCollider.name);
            healthItem.GetDamage(1);
        }

        tearRigidbody2D.velocity = Vector2.zero;
        tearRigidbody2D.SelfCollider.IsTrigger = true;
        frameAnimation.PlayOnce().OnAnimationFinished(() => gameObject.SetActive(false));
    }
}
