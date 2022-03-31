[System.Serializable]
public class CharacterProfileTreeElement : TreeElement
{
    public float BaseMoveSpeed;

    public CharacterProfileTreeElement()
    {
    }

    public CharacterProfileTreeElement(string name, float baseMoveSpeed = 1, int depth = 0, int id = 1) : base(name, depth, id)
    {
        BaseMoveSpeed = baseMoveSpeed;
    }

    public CharacterProfileTreeElement(string name, int depth = 0, int id = 1) : base(name, depth, id)
    {
    }
}
