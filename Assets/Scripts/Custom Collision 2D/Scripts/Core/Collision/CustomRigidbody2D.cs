﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace CustomPhysics2D
{
    public class CustomRigidbody2D : CustomCollisionController
    {
        public enum CollisionDetectionMode
        {
            WhenMoving = 0,
            Continuous = 1
        }

        public float gravityScale = 1.0f;

        [SerializeField] private CollisionDetectionMode collisionDetectionMode;

        [DisplayOnly, SerializeField] private Vector2 _velocity;
        private CollisionInfo2D _collisionInfo;
        private CollisionInfo2D _triggerInfo;
        private bool _colliderIsTrigger = false;
        private CustomPhysicsManager _physicsManager;
        private Vector3 _movement = Vector3.zero;
        private HashSet<CustomCollider2D> _currentDetectionHitTriggers = new HashSet<CustomCollider2D>();
        private HashSet<CustomCollider2D> _currentDetectionHitColliders = new HashSet<CustomCollider2D>();
        private Vector2 _raycastDirection;

        #region Properties
        public Vector2 velocity { get => _velocity; set => _velocity = value; }

        public bool Bounce { get => needBounce; set => needBounce = value; }
        #endregion

        #region Unity Action

        protected override void Awake()
        {
            base.Awake();
            _physicsManager = CustomPhysicsManager.instance;
            collisionMask = _physicsManager.setting.GetCollisionMask(gameObject.layer);

            // Add this.collider to ignoredColliders list
            _ignoredColliders.Add(_collider2D);
        }
        private void OnEnable()
        {
            _physicsManager.PushRigidbody(this);
            UpdateRect();

            onCollisionEnter += DoBounce;
            //onCollisionEnter += SplitCollision;
        }

        private void OnDisable()
        {
            _physicsManager.RemoveRigidbody(this);

            onCollisionEnter -= DoBounce;
            //onCollisionEnter -= SplitCollision;
        }
        private void OnDestroy()
        {
            _physicsManager.RemoveRigidbody(this);
        }
        #endregion

        #region RigidBody2D Basic

        public override void Simulate(float deltaTime)
        {
            base.Simulate(deltaTime);

            // Add velocity generated by gravity
            var gravity = _physicsManager.setting.Gravity;
            var gravityRatio = gravityScale * deltaTime;
            _velocity += gravity * gravityRatio;

            // Movement
            _movement = _velocity * deltaTime;

            // Reset Collision Info Before Collision
            ResetStatesBeforeCollision();

            if (_collider2D == null || !_collider2D.enabled)
            {
                return;
            }

            CollisionDetect();
            Move();

            //// Landing Platform
            //if( !_collisionInfo.isBelowCollision )
            //{
            //	_landingPlatform = null;
            //}

            FixInsertion();
            FixVelocity();

            // Reset Collision Info After Collision
            ResetStatesAfterCollision();
        }

        private void ResetStatesBeforeCollision()
        {
            _colliderIsTrigger = _collider2D.IsTrigger;
            _collisionInfo.Reset();
            _triggerInfo.Reset();
            _raycastOrigins.Reset();
        }

        private void ResetStatesAfterCollision()
        {
        }

        private void CollisionDetect()
        {
            Profiler.BeginSample("CollisionDetect");
            // Clear Hit Triggers
            _currentDetectionHitTriggers.Clear();
            _currentDetectionHitColliders.Clear();

            // Prepare Collision Info
            PrepareCollisionInfo();

            if (float.IsNaN(_movement.x))
            {
                _movement.x = 0.0f;
            }
            if (float.IsNaN(_movement.y))
            {
                _movement.y = 0.0f;
            }

            // Horizontal
            HorizontalCollisionDetect();

            // Vertical
            VerticalCollisionDetect();
            Profiler.EndSample();
        }

        private void Move()
        {
            if (_collider2D == null || !_collider2D.enabled)
            {
                return;
            }

            MovePosition(ref _movement);
            UpdateRect();
        }

        private void FixInsertion()
        {
            _currentDetectionHitColliders.Clear();
        }

        private void FixVelocity()
        {
            switch (_collisionInfo.HitDirection)
            {
                // The Horizontal velocity should be zero if the rigidbody is facing some 'solid' collider.
                case HitColliderDirection.Left when _velocity.x < 0f:
                case HitColliderDirection.Right when _velocity.x > 0f:
                    _velocity.x = 0.0f;
                    break;
                // The Vertical velocity should be zero if the rigidbody is facing some 'solid' collider.
                case HitColliderDirection.Up when _velocity.y > 0f:
                case HitColliderDirection.Down when _velocity.y < 0f:
                    _velocity.y = 0.0f;
                    break;
                default:
                    break;
            }
        }

        private void MovePosition(ref Vector3 movement)
        {
            if (float.IsNaN(movement.x))
            {
                movement.x = 0.0f;
            }

            if (float.IsNaN(movement.y))
            {
                movement.y = 0.0f;
            }

            _transform.position += movement;
        }

        private void PrepareCollisionInfo()
        {
            UpdateRaycastOrigins();
        }

        private void HorizontalCollisionDetect()
        {
            int detectionCount = 1; //检测次数，运动时只检测运动方向
            if (_movement.x == 0)
            {
                if (collisionDetectionMode == CollisionDetectionMode.WhenMoving) return;

                detectionCount = 2;
            }

            var directionX = _movement.x >= 0 ? Vector2.right : Vector2.left;
            HitColliderDirection hitDirection;

            for (int cnt = 0; cnt < detectionCount; cnt++)
            {
                directionX = cnt == 0 ? directionX : -directionX;   //优先检测移动方向
                hitDirection = directionX.ToHitColliderDirection();
                var rayOrigin = (directionX == Vector2.right) ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
                var rayLength = Mathf.Abs(_movement.x) + _shrinkWidth;
                if (_movement.x == 0f)
                {
                    rayLength += _minRayLength;
                }

                for (int i = 0; i < horizontalRayCount; i++)
                {
                    _raycastDirection = directionX;

                    Debug.DrawLine(rayOrigin, rayOrigin + _raycastDirection * rayLength, Color.red);
                    if (CustomPhysicsManager.useUnityRayCast)
                    {
                        var hitCount = Physics2D.RaycastNonAlloc(rayOrigin, _raycastDirection, _raycastHit2D, rayLength, collisionMask);
                        for (int j = 0; j < hitCount; j++)
                        {
                            var hit = _raycastHit2D[j];
                            if (_ignoredColliders.Contains(hit.collider.ToCustomCollider2D())) continue;

                            HandleHitResult(hit.collider.ToCustomCollider2D(), hit.point, hitDirection);
                            ReviseMovement(hit.distance, hitDirection);
                        }
                    }
                    else
                    {
                        _jraycastHitList.Clear();
                        CustomPhysics2D.Raycast(CustomPhysicsManager.instance.SelfQuadTree, rayOrigin, _raycastDirection, ref _jraycastHitList, rayLength, collisionMask);
                        for (int j = 0; j < _jraycastHitList.count; j++)
                        {
                            var hit = _jraycastHitList[j];
                            if (_ignoredColliders.Contains(hit.collider)) continue;

                            HandleHitResult(hit.collider, hit.point, hitDirection);
                            if (!(hit.collider.IsTrigger || _colliderIsTrigger))
                                ReviseMovement(hit.distance, hitDirection);
                        }
                    }
                    rayOrigin.y += _horizontalRaySpace;
                }
            }
        }

        private void VerticalCollisionDetect()
        {
            int detectionCount = 1;
            if (_movement.y == 0)
            {
                if (collisionDetectionMode != CollisionDetectionMode.WhenMoving)
                {
                    detectionCount = 2;
                }
            }

            var directionY = _movement.y > 0 ? Vector2.up : Vector2.down;
            HitColliderDirection hitDirection;

            for (int d = 0; d < detectionCount; d++)
            {
                directionY = d == 0 ? directionY : -directionY;
                hitDirection = directionY.ToHitColliderDirection();
                var rayOrigin = (directionY == Vector2.up) ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;
                rayOrigin.x += _movement.x;

                var rayLength = Mathf.Abs(_movement.y) + _shrinkWidth;
                if (_movement.y == 0f)
                {
                    rayLength += _minRayLength;
                }
                for (int i = 0; i < verticalRayCount; i++)
                {
                    _raycastDirection = directionY;

                    Debug.DrawLine(rayOrigin, rayOrigin + _raycastDirection * rayLength, Color.red);
                    if (CustomPhysicsManager.useUnityRayCast)
                    {
                        var hitCount = Physics2D.RaycastNonAlloc(rayOrigin, _raycastDirection, _raycastHit2D, rayLength, collisionMask);
                        for (int j = 0; j < hitCount; j++)
                        {
                            var hit = _raycastHit2D[j];
                            // Ignored Collider?
                            if (_ignoredColliders.Contains(hit.collider.ToCustomCollider2D())) continue;

                            HandleHitResult(hit.collider.ToCustomCollider2D(), hit.point, hitDirection);
                            ReviseMovement(hit.distance, hitDirection);
                        }
                    }
                    else
                    {
                        _jraycastHitList.Clear();
                        CustomPhysics2D.Raycast(CustomPhysicsManager.instance.SelfQuadTree, rayOrigin, _raycastDirection, ref _jraycastHitList, rayLength, collisionMask);
                        for (int j = 0; j < _jraycastHitList.count; j++)
                        {
                            var hit = _jraycastHitList[j];
                            if (_ignoredColliders.Contains(hit.collider)) continue;

                            HandleHitResult(hit.collider, hit.point, hitDirection);
                            if (!(hit.collider.IsTrigger || _colliderIsTrigger))
                                ReviseMovement(hit.distance, hitDirection);
                        }
                    }

                    rayOrigin.x += _verticalRaySpace;
                }
            }
        }

        private void HandleHitResult(CustomCollider2D hitCollider, Vector2 hitPoint, HitColliderDirection hitDirection)
        {
            if (hitCollider.IsTrigger || _colliderIsTrigger)
                HitTrigger(hitCollider, hitPoint, hitDirection);
            else
                HitCollider(hitCollider, hitPoint, hitDirection);
        }

        /// <summary>
        /// 修正移动速度，防止速度过快穿过碰撞体
        /// </summary>
        /// <param name="hitDistance"></param>
        /// <param name="hitDirection"></param>
        private void ReviseMovement(float hitDistance, HitColliderDirection hitDirection)
        {
            switch (hitDirection)
            {
                case HitColliderDirection.Left:
                case HitColliderDirection.Right:
                    if (_movement.x != 0.0f)
                    {
                        if (Mathf.Abs(hitDistance - _shrinkWidth) < Mathf.Abs(_movement.x))
                        {
                            _movement.x = (hitDistance - _shrinkWidth) * hitDirection.GetMagnitude();
                        }
                    }
                    break;
                case HitColliderDirection.Up:
                case HitColliderDirection.Down:
                    if (_movement.y != 0.0f)
                    {
                        if (Mathf.Abs(hitDistance - _shrinkWidth) < Mathf.Abs(_movement.y))
                        {
                            _movement.y = (hitDistance - _shrinkWidth) * hitDirection.GetMagnitude();
                        }
                    }
                    break;
            }
        }

        private void HitCollider(CustomCollider2D hitCollider, Vector2 hitPoint, HitColliderDirection hitDirection)
        {
            // Collision Info
            _collisionInfo.collider = _collider2D;
            _collisionInfo.hitCollider = hitCollider;
            _collisionInfo.position = hitPoint;

            // Collision Direction
            _collisionInfo.HitDirection = hitDirection;

            // Need Push Collision ?
            if (!_currentDetectionHitColliders.Contains(hitCollider))
            {
                _physicsManager.PushCollision(_collisionInfo);
                _currentDetectionHitColliders.Add(hitCollider);
            }
        }

        private void HitTrigger(CustomCollider2D hitCollider, Vector2 point, HitColliderDirection direction)
        {
            _triggerInfo.collider = _collider2D;
            _triggerInfo.hitCollider = hitCollider;
            _triggerInfo.position = point;

            _triggerInfo.HitDirection = direction;

            if (!_currentDetectionHitTriggers.Contains(hitCollider))
            {
                _physicsManager.PushCollision(_triggerInfo);
                _currentDetectionHitTriggers.Add(hitCollider);
            }
        }
        #endregion

        #region Bounce
        [SerializeField] private bool needBounce = false;
        [Header("Bounce Debug"), SerializeField] private bool showLog = true;
        /// <summary>
        /// 使刚体运动的力
        /// </summary>
        private Vector2 innerForce = Vector2.zero;
        private Vector2 outerForce = Vector2.zero;
        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        private Coroutine Cor_Deceleration;

        public CustomRigidbody2D AddForce(Vector2 force, bool highSpeed = false)
        {
            outerForce = force;

            if (Cor_Deceleration != null)
            {
                StopCoroutine(Cor_Deceleration);
            }
            Cor_Deceleration = StartCoroutine(Deceleration(highSpeed));

            return this;
        }
        private void DoBounce(CollisionInfo2D collisionInfo)
        {
            if (!needBounce) return;
            if (showLog) Debug.Log($"Object name: {name}");
            if (innerForce == Vector2.zero || outerForce != Vector2.zero) return;

            var direction = collisionInfo.HitDirection;

            float angle = Mathf.Atan2(innerForce.y, innerForce.x);

            if (showLog) Debug.Log($"angle: {angle * Mathf.Rad2Deg}");

            switch (direction)
            {
                case HitColliderDirection.Left:
                case HitColliderDirection.Right:
                    innerForce = MyMath.LinePoint.RotateAroundOrigin(innerForce, Mathf.PI - 2 * angle);
                    break;
                case HitColliderDirection.Up:
                case HitColliderDirection.Down:
                    innerForce = MyMath.LinePoint.RotateAroundOrigin(innerForce, -2 * angle);
                    break;
                default:
                    break;
            }

            if (showLog) Debug.Log($"force: {innerForce}");
        }

        private void SplitCollision(CollisionInfo2D collisionInfo)
        {
            if (collisionInfo.hitCollider.SelfBounds.Contains(transform.position))
            {
                var direction = (collisionInfo.hitCollider.transform.position - transform.position).normalized;
                AddForce(direction);
                collisionInfo.hitCollider.GetComponent<CustomRigidbody2D>().AddForce(-direction);
            }
        }

        private UnityEvent onDecelerationBegin = new UnityEvent();
        public CustomRigidbody2D OnDecelerationBegin(UnityAction action)
        {
            onDecelerationBegin.AddListener(action);
            return this;
        }
        private UnityEvent onDecelerationFinish = new UnityEvent();
        public CustomRigidbody2D OnDecelerationFinish(UnityAction action)
        {
            onDecelerationFinish.AddListener(action);
            return this;
        }
        IEnumerator Deceleration(bool highSpeed)
        {
            var v = Vector2.zero;
            _velocity = innerForce = outerForce;
            if (showLog) Debug.Log($"outerForce: {outerForce}, innerForce: {innerForce}, frames: {Time.frameCount}");
            yield return waitForFixedUpdate;
            outerForce = Vector2.zero;
            if (showLog) Debug.Log($"frames: {Time.frameCount}");

            onDecelerationBegin.Invoke();
            onDecelerationBegin.RemoveAllListeners();
            do
            {
                _velocity = innerForce;
                yield return waitForFixedUpdate;

                innerForce = Vector2.SmoothDamp(innerForce, Vector2.zero, ref v, highSpeed ? 0.1f : 0.5f);
            }
            while (Mathf.Abs(innerForce.x) > 0.05f || Mathf.Abs(innerForce.y) > 0.05f);

            onDecelerationFinish.Invoke();
            onDecelerationFinish.RemoveAllListeners();
            outerForce = innerForce = _velocity = Vector2.zero;
        }
        #endregion
    }
}
