using CustomPhysics2D;
using UnityEngine;

public class Coin : PickupObject
{
    [SerializeField] private CustomFrameAnimation highLight;
    [SerializeField] private CoinSO coinSO;

    private CoinSO.CoinType coinType;
    private int coinWorth;

    public override void OnCustomCollisionEnter(CollisionInfo2D collisionInfo)
    {
        base.OnCustomCollisionEnter(collisionInfo);

    }

    public override void ResetObject()
    {
        base.ResetObject();

        highLight.Play();
         coinType = coinSO.GenerateType();
        objectRenderer.sprite = coinSO.CoinSprites[(int)coinType];
        coinWorth = CoinSO.CoinWorths[(int)coinType];
    }

    protected override void OnPlayerCollect()
    {
        gamePlayer.GetCoin(coinWorth);

        highLight.Stop();
    }
}
