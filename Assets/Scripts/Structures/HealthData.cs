﻿using System;

[Serializable]
public struct HealthData : IComparable<HealthData>
{
    public int RedHeartContainers;
    public int RedHeart;
    public int SoulHeart;
    public int WhiteHeart;

    public static HealthData Zero { get; }

    public static HealthData RedOne => new HealthData(1, 0);
    public static HealthData SoulOne => new HealthData(0, 1);

    public HealthData(int redHeart, int soulHeart, int whiteHeart = 0)
    {
        RedHeart = RedHeartContainers = redHeart;
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
            if (RedHeart == RedHeartContainers)
                return false;
            else
            {
                RedHeart = Math.Min(RedHeart + redHeart, RedHeartContainers);
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

        return a;
    }
    public static HealthData operator +(HealthData a, HealthData b)
    {
        a.RedHeart = Math.Min(a.RedHeartContainers, a.RedHeart + b.RedHeart);
        a.SoulHeart += b.SoulHeart;
        a.WhiteHeart += b.WhiteHeart;
        if ((a.WhiteHeart & 1) == 0)
        {
            a.RedHeartContainers = a.RedHeart += a.WhiteHeart >> 1;
            a.WhiteHeart = 0;
        }
        return a;
    }
}