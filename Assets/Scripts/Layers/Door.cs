using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [HideInInspector]
    public DoorPosition doorPosition;

    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private SpriteRenderer doorFrameSprite;
    [SerializeField] private SpriteRenderer doorLockSprite;
    [SerializeField] private DoorObjectType doorType;

    [SerializeField] private DoorPositionEventChannelSO onEnterDoorEvent;
    [SerializeField] private UnityAction<DoorStatus> onDoorStatusChanged;

    private Animation doorAnimation;
    private CustomFrameAnimation smokeAnimation;
    private CustomCollisionController collisionController;

    private DoorStatus doorStatus;

    private Bounds doorBound;

    public DoorObjectType DoorType { get => doorType; }

    private void OnEnable()
    {
        doorStatus = DoorStatus.Closed;
        onDoorStatusChanged += OnDoorStatusChanged;
        collisionController.onTriggerEnter += TriggerEnter;
    }

    private void OnDisable()
    {
        onDoorStatusChanged -= OnDoorStatusChanged;
        collisionController.onTriggerEnter -= TriggerEnter;
    }

    private void Awake()
    {
        collisionController = GetComponent<CustomCollisionController>();
        doorAnimation = GetComponent<Animation>();
        smokeAnimation = GetComponentInChildren<CustomFrameAnimation>();
    }

    private void TriggerEnter(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.CompareTag("Player"))
            onEnterDoorEvent.RaiseEvent(doorPosition);
    }

    public static (Vector3 localPosition, Quaternion rotation) GetDoorTransform(DoorPosition type)
    {
        switch (type)
        {
            case DoorPosition.Up:
                return (new Vector3(0, 3.77f), Quaternion.identity);
            case DoorPosition.Down:
                return (new Vector3(0, -3.77f), Quaternion.Euler(0, 0, 180));
            case DoorPosition.Left:
                return (new Vector3(-6.6f, 0), Quaternion.Euler(0, 0, 90));
            case DoorPosition.Right:
                return (new Vector3(6.6f, 0), Quaternion.Euler(0, 0, -90));
            default:
                return (Vector3.zero, Quaternion.identity);
        }
    }

    public void OpenDoor()
    {
        //TODO
        doorStatus = DoorStatus.Open;
        doorAnimation.Play(string.Format("Door_{0}_{1}", doorType, doorStatus));
        switch (doorType)
        {
            case DoorObjectType.BossChallenge:
            case DoorObjectType.Challenge:
            case DoorObjectType.Devil:
            case DoorObjectType.Angel:
                smokeAnimation?.PlayOnce();
                break;
        }
    }

    public void ResetDoor()
    {
        doorStatus = DoorStatus.Closed;
        doorAnimation.Play(string.Format("Door_{0}_{1}", doorType, doorStatus));
    }

    private void BreakDoor()
    {
        doorStatus = DoorStatus.Broken;
        throw new NotImplementedException();
    }

    public void RaiseEvent(DoorStatus status)
    {
        onDoorStatusChanged.Invoke(status);
    }

    private void OnDoorStatusChanged(DoorStatus arg0)
    {
        switch (arg0)
        {
            case DoorStatus.Closed:
                ResetDoor();
                break;
            case DoorStatus.Open:
                OpenDoor();
                break;
            case DoorStatus.Broken:
                BreakDoor();
                break;
        }
    }
}
