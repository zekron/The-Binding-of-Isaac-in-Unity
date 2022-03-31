[System.Serializable]
public class PlayerProfileTreeElement : CharacterProfileTreeElement
{
    public HealthData PlayerHealthData;
    /// <summary>
    /// �����˺�
    /// </summary>
    public float BaseDamage;
    public float DamageMultiplier;
    /// <summary>
    /// ��������
    /// </summary>
    public float BaseRange;
    /// <summary>
    /// ����Ƶ��
    /// </summary>
    public float Tears;
    public float TearDelay;
    /// <summary>
    /// �����ٶ�
    /// </summary>
    public float ShotSpeed;
    /// <summary>
    /// ����ֵ
    /// </summary>
    public float Luck;

    public float KnockBack;

    public int CoinCount;
    public int BombCount;
    public int KeyCount;

    public PlayerProfileTreeElement()
    {
    }

    public PlayerProfileTreeElement(string name, HealthData healthData, int depth = 0, int id = 1, float baseMoveSpeed = 1, float baseDamage = 0, float damageMultiplier = 0, float baseRange = 0, float tears = 0, float tearDelay = 0, float shotSpeed = 0, float luck = 0, int coinCount = 0, int bombCount = 0, int keyCount = 0) : base(name, baseMoveSpeed, depth, id)
    {
        PlayerHealthData = healthData;

        BaseDamage = baseDamage;
        DamageMultiplier = damageMultiplier;
        BaseRange = baseRange;
        Tears = tears;
        TearDelay = tearDelay;
        ShotSpeed = shotSpeed;
        Luck = luck;
        CoinCount = coinCount;
        BombCount = bombCount;
        KeyCount = keyCount;
    }

}
