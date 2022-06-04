using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : RoomObject, IHealth
{
    [SerializeField] private Vector3[] fireSizes;
    [SerializeField] private SpriteRenderer fire;
    [SerializeField] private SpriteRenderer fireShadow;
    [SerializeField] private Sprite[] firePlaceSprites;

    private int maxHealth;
    private int currentHealth;

    public int Health => currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth = fireSizes.Length - 1;
    }
    public override void ChangeRendererOrder()
    {
        base.ChangeRendererOrder();

        fireShadow.sortingOrder = objectRenderer.sortingOrder - 1;
        fire.sortingOrder = objectRenderer.sortingOrder + 1;
    }
    public void DestroySelf()
    {
        platform.SelfCollider.IsTrigger = true;
        fire.enabled = false;
        fireShadow.enabled = false;
        objectRenderer.sprite = firePlaceSprites[0];
    }

    public void GetDamage(int damage)
    {
        if (currentHealth <= 0) return;

        if (Random.value < 0.3) damage += Random.Range(0, 2);
        currentHealth = Mathf.Max(0, currentHealth - damage);
        fire.transform.localScale = fireSizes[currentHealth];

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
        ObjectRenderer.sprite = firePlaceSprites[1];

        fire.enabled = true;
        fire.transform.localScale = fireSizes[currentHealth];
        fire.GetComponent<CustomFrameAnimation>().ResetAnimation();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
