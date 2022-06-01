using UnityEngine;

public class TNT : RoomObject, IHealth
{
    [SerializeField] private int maxHealth;
    [SerializeField] private CustomFrameAnimationClip objectClip;

    private int currentHealth;

    public int Health => currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
    }

    public void DestroySelf()
    {
        platform.SelfCollider.IsTrigger = true;
    }

    public void GetDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        objectRenderer.sprite = objectClip.NextFrame();

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
        objectClip.ResetClip();
        objectRenderer.sprite = objectClip.CurrentFrame();
    }
}
