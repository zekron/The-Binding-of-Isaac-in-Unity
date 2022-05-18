using UnityEngine;
using System.Collections;
using System;

namespace J2P
{
    public class JPlatform : JCollisionController
    {
        protected override void Awake()
        {
            base.Awake();
            JPhysicsManager.instance.PushPlatform(this);
        }

        private void OnEnable()
        {
            JPhysicsManager.instance.PushPlatform(this);
            onCollisionEnter += CollisionEnter;
            onCollisionExit += CollisionExit;
        }

        private void OnDisable()
        {
            JPhysicsManager.instance.RemovePlatform(this);
            onCollisionEnter -= CollisionEnter;
            onCollisionExit -= CollisionExit;
        }

        private void CollisionEnter(CollisionInfo collisionInfo)
        {
            Debug.Log(name + " enter " + collisionInfo.position);
        }

        private void CollisionExit(CollisionInfo collisionInfo)
        {
            Debug.Log(name + " exit " + collisionInfo.position);
        }
    }
}
