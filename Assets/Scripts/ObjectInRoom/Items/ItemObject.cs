using CustomPhysics2D;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    [SerializeField] protected int itemID;

    protected ItemTreeElement itemData;
    protected Player gamePlayer;

    private SpriteRenderer itemRenderer;

    public SpriteRenderer ItemRenderer => itemRenderer = GetComponentInChildren<SpriteRenderer>();

    private void Awake()
    {
        itemRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Collect(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gamePlayer))
        {
            //pickupAnimation.Play("Pickup_OnPicked");

            OnPlayerCollect();

            gameObject.SetActive(false);
        }
    }

    protected abstract void OnPlayerCollect();
}
