using AssetBundleFramework;
using CustomPhysics2D;
using UnityEngine;

public class Chest : RoomObject
{
    [SerializeField] private ChestSO chestSO;

    private ChestSO.ChestType chestType;
    protected Player gamePlayer;
    private CustomFrameAnimation openAnimation;

    private bool isOpen = false;

    public void SetType(ChestSO.ChestType type)
    {
        chestType = type;

        objectRenderer.sprite = chestSO.ChestSprites[(int)type];
        openAnimation.ChangeClip(chestSO.OpenChestClips[(int)type]);
    }

    protected override void Awake()
    {
        base.Awake();

        chestSO = AssetBundleManager.Instance.LoadAsset<ChestSO>("ChestSO.asset");
        openAnimation = GetComponent<CustomFrameAnimation>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        collisionController.onCollisionEnter += OnCustomCollisionEnter;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        collisionController.onCollisionEnter -= OnCustomCollisionEnter;
    }

    private void OnCustomCollisionEnter(CollisionInfo2D collisionInfo)
    {
        if (collisionInfo.hitCollider.TryGetComponent(out gamePlayer))
        {
            var direction = (transform.position - collisionInfo.hitCollider.transform.position).normalized;
            (collisionController as CustomRigidbody2D).AddForce(direction);

            if (isOpen) return;

            if (chestType == ChestSO.ChestType.Locked)
            {
                if (gamePlayer.KeyCount < 1) return;

                if (!gamePlayer.GetGoldenKey)
                    gamePlayer.UseKey(1);
            }
            openAnimation.Play();
            chestSO.SpawnReward(chestType, transform.position);
            isOpen = true;
        }
    }

    public override void ResetObject()
    {
        openAnimation.ResetAnimation();
        isOpen = false;

        SetType(chestSO.GenerateType());
    }
}
