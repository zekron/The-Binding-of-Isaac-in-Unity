using System;

[Serializable]
public struct HealthData : IComparable<HealthData>
{
    public int RedHeartContainers;
    public int RedHeart;
    public int SoulHeart;

    public static HealthData Zero { get; }

    public static HealthData RedOne { get => new HealthData(1, 0); }
    public static HealthData SoulOne { get => new HealthData(0, 1); }

    public HealthData(int redHeart, int soulHeart)
    {
        RedHeart = RedHeartContainers = redHeart;
        SoulHeart = soulHeart;
    }

    public bool Initialze(int redHeart, int soulHeart)
    {
        try
        {
            RedHeartContainers = RedHeart = redHeart;
            SoulHeart = soulHeart;
        }
        catch (Exception exceprtion)
        {
            UnityEngine.Debug.LogError(exceprtion);
            return false;
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
}
