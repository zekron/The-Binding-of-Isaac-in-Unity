using System;

[Serializable]
public struct HealthData : IComparable<HealthData>
{
    public int RedHeartContainers;
    public int RedHeart;
    public int SoulHeart;
    public int WhiteHeart;

    public static HealthData Zero { get; }

    public static HealthData RedHalf => new HealthData(1, 0);
    public static HealthData RedOne => new HealthData(2, 0);
    public static HealthData SoulOne => new HealthData(0, 2);
    public static HealthData WhiteHalf => new HealthData(0, 0, 1);

    public HealthData(int redHeart, int soulHeart, int whiteHeart = 0)
    {
        RedHeart = redHeart;
        RedHeartContainers = RedHeart / 2 + RedHeart % 2;
        SoulHeart = soulHeart;
        WhiteHeart = whiteHeart;
    }

    public bool RefreshData(int heartContainer, int redHeart, int soulHeart)
    {
        try
        {
            RedHeartContainers = heartContainer;
            RedHeart = redHeart;
            SoulHeart = soulHeart;
        }
        catch (Exception exceprtion)
        {
            UnityEngine.Debug.LogError(exceprtion);
            return false;
        }
        return true;
    }

    public bool GetHeart(int redHeart, int soulHeart, int whiteHeart)
    {
        if (redHeart > 0)
        {
            if (RedHeart == RedHeartContainers * 2)
                return false;
            else
            {
                RedHeart = Math.Min(RedHeart + redHeart, RedHeartContainers * 2);
            }
        }
        SoulHeart += soulHeart;
        WhiteHeart += whiteHeart;
        if ((WhiteHeart & 1) == 0)
        {
            RedHeartContainers = RedHeart += WhiteHeart >> 1;
            WhiteHeart = 0;
        }

        return true;
    }

    public int CompareTo(HealthData other)
    {
        if (RedHeart > other.RedHeart)
            return 1;
        else if (RedHeart == other.RedHeart)
        {
            if (SoulHeart > other.SoulHeart)
                return 1;
            else
                return -1;
        }
        else
            return -1;
    }

    public static bool operator ==(HealthData a, HealthData b) => a.RedHeart == b.RedHeart
            && a.SoulHeart == b.SoulHeart
            && a.WhiteHeart == b.WhiteHeart;

    public static bool operator !=(HealthData a, HealthData b) => a.RedHeart != b.RedHeart
            || a.SoulHeart != b.SoulHeart
            || a.WhiteHeart != b.WhiteHeart;

    public static HealthData operator -(HealthData a, int b)
    {
        a.SoulHeart -= b;
        if (a.SoulHeart < 0)
        {
            b = -a.SoulHeart;
            a.SoulHeart = 0;
        }
        a.WhiteHeart -= b;
        if (a.WhiteHeart < 0)
        {
            b = -a.WhiteHeart;
            a.WhiteHeart = 0;
        }
        a.RedHeart -= b;
        if (a.RedHeart <= 0)
            a.RedHeart = 0;

        return a;
    }
    public static HealthData operator +(HealthData a, HealthData b)
    {
        a.RedHeart = Math.Min(a.RedHeartContainers * 2, a.RedHeart + b.RedHeart);
        a.SoulHeart += b.SoulHeart;
        a.WhiteHeart += b.WhiteHeart;
        if (a.WhiteHeart != 0 && a.WhiteHeart % 2 == 0)
        {
            a.RedHeartContainers += 1;
            a.RedHeart += 2;
            a.WhiteHeart = 0;
        }
        return a;
    }
}
