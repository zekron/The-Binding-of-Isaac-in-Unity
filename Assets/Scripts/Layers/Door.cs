using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private SpriteRenderer doorFrameSprite;
    [SerializeField] private SpriteRenderer doorLockSprite;

    [SerializeField] private UnityAction<DoorStatus> onDoorStatusChanged;

    private Animation doorAnimation;

    private DoorStatus doorStatus;
    private DoorType doorType;

    private void OnEnable()
    {
        doorStatus = DoorStatus.Closed;
        onDoorStatusChanged += OnDoorStatusChanged;
    }

    private void OnDisable()
    {
        onDoorStatusChanged -= OnDoorStatusChanged;
    }

    private void OnDoorStatusChanged(DoorStatus arg0)
    {
        switch (arg0)
        {
            case DoorStatus.Closed:
                DoorReset();
                break;
            case DoorStatus.Open:
                DoorOpen();
                break;
            case DoorStatus.Broken:
                DoorBroken();
                break;
        }
    }

    private void Awake()
    {
        doorAnimation = GetComponent<Animation>();
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

    public void DoorOpen()
    {
        //TODO
        doorStatus = DoorStatus.Open;
        doorAnimation.Play("Door_Normal_Open");
    }

    public void DoorReset()
    {
        doorStatus = DoorStatus.Closed;
        doorAnimation.Play("Door_Normal_Idle");
    }

    private void DoorBroken()
    {
        doorStatus = DoorStatus.Broken;
        throw new NotImplementedException();
    }

    public void RaiseEvent(DoorStatus status)
    {
        onDoorStatusChanged.Invoke(status);
    }
}
