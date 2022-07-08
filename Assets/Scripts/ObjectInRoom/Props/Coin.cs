using CustomPhysics2D;
using UnityEngine;

public class Coin : PickupObject
{
    [SerializeField] private CustomFrameAnimation highLight;
    [SerializeField] private CoinSO coinSO;

    private SpriteRenderer coinRenderer;
    private int coinWorth;

    protected override void Awake()
    {
        base.Awake();

        coinRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Collect(CollisionInfo2D collisionInfo)
    {
        base.Collect(collisionInfo);

        highLight.Stop();
    }

    public override void ResetObject()
    {
        base.ResetObject();

        highLight.Play();
        var type = coinSO.GenerateType();
        coinRenderer.sprite = coinSO.CoinSprites[(int)type];
        coinWorth = CoinSO.CoinWorths[(int)type];
    }
}
