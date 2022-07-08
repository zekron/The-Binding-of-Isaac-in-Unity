using System.Collections;
using System.Collections.Generic;
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
        return (CoinType)Random.Range(0, 3);
    }
}
