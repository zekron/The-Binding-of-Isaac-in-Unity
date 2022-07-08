using UnityEngine;

[CreateAssetMenu(fileName = "BombSO", menuName = "Scriptable Object/Pickups/Bomb")]
public class BombSO : ScriptableObject
{
    public Sprite[] BombSprites;

    public readonly static int[] BombWorth = new int[] { 1, 2, 0, 0 };

    public enum BombType
    {
        Single,
        Double,
        Troll,
        MegaTroll,
    }

    public BombType GenerateType()
    {
        var rnd = Random.value;

        if (rnd < 7f / 10) return BombType.Single;
        else if (rnd < 8f / 10) return BombType.Double;
        else if (rnd < 9f / 10) return BombType.Troll;
        else return BombType.MegaTroll;
    }
}
