using UnityEngine;

[CreateAssetMenu(fileName = "KeySO", menuName = "Scriptable Object/Pickups/Key")]
public class KeySO : ScriptableObject
{
    public Sprite[] KeySprites;

    public readonly static int[] KeyWorths = new int[] { 1, 0 };

    public enum KeyType
    {
        Normal,
        Golden,
    }

    public KeyType GenerateType()
    {
        var rnd = Random.value;

        if (rnd < 9.5f / 10) return KeyType.Normal;
        else return KeyType.Golden;
    }
}
