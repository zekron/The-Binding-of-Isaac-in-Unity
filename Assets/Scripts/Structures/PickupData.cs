using System;

[Serializable]
public struct PickupData
{
    private int coin;
    private int key;
    private int bomb;

    public int Coin => coin;
    public int Key => key;
    public int Bomb => bomb;
    public static PickupData zero => new PickupData(0, 0, 0);

    public PickupData(int coin, int key, int bomb)
    {
        this.coin = coin;
        this.key = key;
        this.bomb = bomb;
    }

    public void RefreshData(int coin, int key, int bomb)
    {
        this.coin = coin;
        this.key = key;
        this.bomb = bomb;
    }
}
