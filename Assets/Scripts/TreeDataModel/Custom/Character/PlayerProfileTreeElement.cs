using UnityEngine;

[System.Serializable]
public class PlayerProfileTreeElement : CharacterProfileTreeElement
{
    public HealthData PlayerHealthData;
    public PickupData PlayerPickupData;
    public ItemTreeElement PlayerItemData;
    /// <summary>
    /// »ù´¡ÉËº¦
    /// </summary>
    public float BaseDamage;
    public float DamageMultiplier;
    /// <summary>
    /// ¹¥»÷¾àÀë
    /// </summary>
    public float BaseRange;
    /// <summary>
    /// ¹¥»÷ÆµÂÊ
    /// <para>T = 30 / (d + 1)</para>
    /// </summary>
    public float PlayerTearsAddition;
    /// <summary>
    /// ¹¥»÷¼ä¸ô
    /// </summary>
    public float PlayerTearsMultiplier;
    /// <summary>
    /// µ¯µÀËÙ¶È
    /// </summary>
    public float ShotSpeed;
    /// <summary>
    /// ÐÒÔËÖµ
    /// </summary>
    public float Luck;

    public float KnockBack;

    public int CoinCount;
    public int BombCount;
    public int KeyCount;

    public PlayerProfileTreeElement()
    {
    }

    public PlayerProfileTreeElement(string name, HealthData healthData, PickupData pickupData, ItemTreeElement item, int depth = 0, int id = 1, float baseMoveSpeed = 1, float baseDamage = 0, float damageMultiplier = 0, float baseRange = 0, float tears = 0, float tearDelay = 0, float shotSpeed = 0, float luck = 0, int coinCount = 0, int bombCount = 0, int keyCount = 0) : base(name, baseMoveSpeed, depth, id)
    {
        PlayerHealthData = healthData;
        PlayerPickupData = pickupData;
        PlayerItemData = item;

        BaseDamage = baseDamage;
        DamageMultiplier = damageMultiplier;
        BaseRange = baseRange;
        PlayerTearsAddition = tears;
        PlayerTearsMultiplier = tearDelay;
        ShotSpeed = shotSpeed;
        Luck = luck;
        CoinCount = coinCount;
        BombCount = bombCount;
        KeyCount = keyCount;
    }

    /// <summary>
    /// the approximate damage that can be expected per tear, or per 'tick'
    /// </summary>
    /// <param name="baseDamage">The character base damage is defined by a base number and a multiplier.</param>
    /// <param name="totalDamageUps"> the total of all regular damage ups collected (not including special exceptions or damage multipliers)</param>
    /// <param name="flatDamageUps"> the total of all damage ups that are excluded from the above</param>
    /// <returns></returns>
    public static float GetEffectiveDamage(float baseDamage, float totalDamageUps, float flatDamageUps)
    {
        return baseDamage * Mathf.Sqrt(totalDamageUps * 1.2f + 1) + flatDamageUps;
    }

    /// <summary>
    /// The formula for tear delay
    /// </summary>
    /// <param name="t">the normal tears stat, with all adjustments added together.</param>
    /// <returns></returns>
    public static float GetTearDelay(float t, float addition, float multiplier)
    {
        float result;

        if (t > 425f / 234) result = 5;
        else if (t >= 0 && t < 425f / 234) result = 16 - 6 * Mathf.Sqrt(1.3f * t + 1);
        else if (t < 0 && t > -10f / 13) result = 16 - 6 * Mathf.Sqrt(1.3f * t + 1) - 6 * t;
        else result = 16 - 6 * t;

        result *= (multiplier + 1);
        result += addition;

        return Mathf.Round(result * 100) / 100;
    }
    public float GetTears(float tearDelay)
    {
        float result = 30f / (tearDelay + 1);

        return Mathf.Round(result * 100) / 100;
    }
}

public struct TearsModification
{
    public float TearsAddition;
    public float TearsMultiplier;
}
