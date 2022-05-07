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
    [SerializeField] private AnimationClip[] animationClips;

    [SerializeField] private UnityAction<DoorStatus> onDoorStatusChanged;

    private Animation doorAnimation;

    private DoorStatus doorStatus;
    private DoorType doorType;

    private Bounds doorBound;

    private void OnEnable()
    {
        doorStatus = DoorStatus.Closed;
        onDoorStatusChanged += OnDoorStatusChanged;
    }

    private void OnDisable()
    {
        onDoorStatusChanged -= OnDoorStatusChanged;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        //animationClips = UnityEditor.AnimationUtility.GetAnimationClips(gameObject);
    }
#endif

    private void Awake()
    {
        doorAnimation = GetComponent<Animation>();
        smokeAnimation = GetComponentInChildren<CustomFrameAnimation>();
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
<<<<<<< Updated upstream
        doorAnimation.Play(animationClips[(int)doorStatus].name);
=======
        doorAnimation.Play(string.Format("Door_{0}_{1}", doorType, doorStatus));
        switch (doorType)
        {
            case DoorType.BossChallenge:
            case DoorType.Challenge:
            case DoorType.Devil:
            case DoorType.Angel:
                smokeAnimation?.PlayOnce();
                break;
        }
>>>>>>> Stashed changes
    }

    public void ResetDoor()
    {
        doorStatus = DoorStatus.Closed;
        doorAnimation.Play(animationClips[(int)doorStatus].name);
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
