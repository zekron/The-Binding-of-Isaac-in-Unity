﻿using CustomPhysics2D;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour, IObjectInRoom, IDisplayInEditorWindow
{
    protected CustomCollisionController collisionController;
    protected SpriteRenderer objectRenderer;
    protected GameCoordinate coordinate;

    public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }
    public SpriteRenderer ObjectRenderer => objectRenderer;

    public Sprite SpriteInEditorWindow => GetComponent<SpriteRenderer>().sprite;

    public CustomCollisionController CollisionController  => collisionController;

    protected virtual void Awake()
    {
        collisionController = GetComponent<CustomCollisionController>();
        objectRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        ResetObject();
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void ChangeRendererOrder()
    {
        objectRenderer.sortingOrder = coordinate.y * -5;
    }

    public abstract void ResetObject();
}