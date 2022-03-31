[System.Serializable]
public class EnemyProfileTreeElement : CharacterProfileTreeElement
{
    public int BaseHealth;

    public float BaseDamage;
    public float BaseRange;
    public float TearDelay;

    public enum EnemyType
    {
        Chasing,
        Shooting,
    }

    public EnemyProfileTreeElement()
    {
    }

    public EnemyProfileTreeElement(string name, int baseHealth, int depth = 0, int id = 1, float baseDamage = 1, float tearDelay = 0, float baseMoveSpeed = 1,float baseRange = 0) : base(name, baseMoveSpeed, depth, id)
    {
        BaseHealth = baseHealth;

        BaseDamage = baseDamage;
        BaseRange = baseRange;
        TearDelay = tearDelay;
    }
}
