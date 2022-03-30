using System;

[Serializable]
public class CharacterProfileTreeElement : TreeElement
{
    public HealthData BaseHealth;
    public float BaseMoveSpeed;

    public CharacterProfileTreeElement()
    {
    }

    public CharacterProfileTreeElement(string name, HealthData baseHealth, float baseMoveSpeed = 1, int depth = 0, int id = 1) : base(name, depth, id)
    {
        BaseHealth = baseHealth;
        BaseMoveSpeed = baseMoveSpeed;
    }

    public CharacterProfileTreeElement(string name, int depth = 0, int id = 1) : base(name, depth, id)
    {
    }
}
