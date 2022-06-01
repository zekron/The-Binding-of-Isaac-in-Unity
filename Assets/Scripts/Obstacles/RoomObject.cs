using CustomPhysics2D;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour, IObjectInRoom
{
    protected CustomPlatform2D platform;
    protected SpriteRenderer objectRenderer;
    protected GameCoordinate coordinate;

    public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }
    public SpriteRenderer ObjectRenderer => objectRenderer;

    protected virtual void Awake()
    {
        platform = GetComponent<CustomPlatform2D>();
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