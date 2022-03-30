using System;

[Serializable]
public struct HealthData : IComparable<HealthData>
{
    public int RedHeartContainers { get; }
    public int RedHeart { get; set; }
    public int SoulHeart { get; set; }

    public static HealthData Zero { get; }

    public HealthData(int redHeart, int soulHeart)
    {
        RedHeart = RedHeartContainers = redHeart;
        SoulHeart = soulHeart;
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
