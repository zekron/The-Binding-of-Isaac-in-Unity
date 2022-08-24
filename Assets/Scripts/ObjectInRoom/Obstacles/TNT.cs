using UnityEngine;

public class TNT : RoomObject, IHealth
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

    public void DestroySelf()
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
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + healing);
    }

    public override void ResetObject()
    {
        currentHealth = maxHealth;
        objectRenderer.sprite = objectClip.ResetClip();
    }
}
