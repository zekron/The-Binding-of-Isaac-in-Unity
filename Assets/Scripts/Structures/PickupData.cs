using System;

[Serializable]
public struct PickupData
{
    public int coin;
    public int key;
    public int bomb;

    public bool getGoldenKey;

    public static PickupData zero => new PickupData(0, 0, 0);

    public PickupData(int coin, int key, int bomb, bool getGoldenKey = false)
    {
        this.coin = coin;
        this.key = key;
        this.bomb = bomb;

        this.getGoldenKey = getGoldenKey;
    }

    public void RefreshData(int coin, int key, int bomb, bool getGoldenKey = false)
    {
        this.coin = coin;
        this.key = key;
        this.bomb = bomb;

        this.getGoldenKey = getGoldenKey;
    }

    public static PickupData operator +(PickupData left, PickupData right) => new PickupData(left.coin + right.coin, left.key + right.key, left.bomb + right.bomb);
}
