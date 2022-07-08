using UnityEngine;

[CreateAssetMenu(fileName = "CoinSO", menuName = "Scriptable Object/Pickups/Coin")]
public class CoinSO : ScriptableObject
{
    public Sprite[] CoinSprites;

    public readonly static int[] CoinWorths = new int[] { 1, 5, 10 };

    public enum CoinType
    {
        Penny,
        Nickel,
        Dime,
    }

    public CoinType GenerateType()
    {
        var rnd = Random.value;

        if (rnd < 10f / 16) return CoinType.Penny;
        else if (rnd < 15f / 16) return CoinType.Nickel;
        else return CoinType.Dime;
    }
}
