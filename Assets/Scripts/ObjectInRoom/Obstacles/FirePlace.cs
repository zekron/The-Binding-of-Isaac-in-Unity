using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : RoomObject, IHealth
{
    [SerializeField] private Vector3[] fireSizes;
    [SerializeField] private SpriteRenderer fireRenderer;
    [SerializeField] private SpriteRenderer fireShadow;
    [SerializeField] private Sprite[] firePlaceSprites;

    private CustomFrameAnimation fireAnimation;
    private Player gameplayer;
    private int maxHealth;
    private int currentHealth;
    private int attackValue = 1;

    public int Health => currentHealth;

    protected override void Awake()
    {
        objectRenderer = transform.parent.GetComponent<SpriteRenderer>();
        collisionController = GetComponent<CustomCollisionController>();
        fireAnimation = GetComponent<CustomFrameAnimation>();

        currentHealth = maxHealth = fireSizes.Length - 1;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        collisionController.onCollisionEnter += AttackCharacter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        collisionController.onCollisionEnter -= AttackCharacter;
    }

    public override void ChangeRendererOrder()
    {
        base.ChangeRendererOrder();

        fireShadow.sortingOrder = objectRenderer.sortingOrder - 1;
        fireRenderer.sortingOrder = objectRenderer.sortingOrder + 1;
    }
    public void DestroySelf()
    {
        collisionController.SelfCollider.IsTrigger = true;
        fireRenderer.enabled = false;
        fireShadow.enabled = false;
        objectRenderer.sprite = firePlaceSprites[0];
        fireAnimation.Stop();
    }

    public void GetDamage(int damage)
    {
        if (currentHealth <= 0) return;

        if (Random.value < 0.3) damage += Random.Range(0, 2);
        currentHealth = Mathf.Max(0, currentHealth - damage);
        fireRenderer.transform.localScale = fireSizes[currentHealth];

        if (currentHealth == 0) DestroySelf();
    }

    public void GetHealing(int healing)
    {
        return;
    }

    public override void ResetObject()
    {
        currentHealth = maxHealth;
        fireShadow.enabled = true;
        objectRenderer.sprite = firePlaceSprites[1];

        fireRenderer.enabled = true;
        fireRenderer.transform.localScale = fireSizes[currentHealth];
        fireAnimation.Play();
    }

    public void AttackCharacter(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gameplayer))
        {
            gameplayer.GetDamage(attackValue);
            gameplayer.PlayerRigidbody.AddForce((gameplayer.transform.position - transform.position).normalized, highSpeed: true)
                .OnDecelerationBegin(() => gameplayer.ControllerEnabled = false)
                .OnDecelerationFinish(() => gameplayer.ControllerEnabled = true);
        }
    }
}
