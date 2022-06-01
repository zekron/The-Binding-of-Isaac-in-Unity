using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear : MonoBehaviour
{
    private CustomFrameAnimation frameAnimation;
    private CustomRigidbody2D rigidbody2D;

    public Vector2 MoveVelocity { set => rigidbody2D.velocity = value; }
    private void Awake()
    {
        frameAnimation = GetComponent<CustomFrameAnimation>();
        rigidbody2D = GetComponent<CustomRigidbody2D>();
    }

    private void OnEnable()
    {
        rigidbody2D.onCollisionEnter += HitCollision;
    }
    private void OnDisable()
    {
        rigidbody2D.onCollisionEnter -= HitCollision;
    }

    private void HitCollision(CollisionInfo2D collisionInfo)
    {
        rigidbody2D.SelfCollider.IsTrigger = true;
        frameAnimation.PlayOnce(() => gameObject.SetActive(false));
    }
}
