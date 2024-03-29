using CustomPhysics2D;
using UnityEngine;

public class Poop : RoomObject, IHealth
{
    [SerializeField] private CustomFrameAnimationClip objectClip;

    private int maxHealth;
    private int currentHealth;

    public int Health => currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth = objectClip.FramesCount - 1;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public virtual void DestroySelf()
    {
        collisionController.SelfCollider.IsTrigger = true;
    }

    public void GetDamage(int damage)
    {
        if (currentHealth <= 0) return;

        var currentFrame = maxHealth - currentHealth;
        objectRenderer.sprite = objectClip.NextFrame(ref currentFrame);
        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth == 0) DestroySelf();
    }

    public void GetHealing(int healing)
    {
        return;
    }

    public override void ResetObject()
    {
        currentHealth = maxHealth;
        objectRenderer.sprite = objectClip.ResetClip();
    }
}
