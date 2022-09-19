using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Head Sprite Group", menuName = "Scriptable Object/Sprites/Head Sprite Group")]
public class HeadSpriteGroup : ScriptableObject
{
    [SerializeField] private Sprite[] upShootingSprites;
    [SerializeField] private Sprite[] downShootingSprites;
    [SerializeField] private Sprite[] leftShootingSprites;
    [SerializeField] private Sprite[] rightShootingSprites;

    public const int OPEN_EYE_SPRITE_INDEX = 0;
    public const int CLOSE_EYE_SPRITE_INDEX = 1;

    public Sprite GetSprite(Vector2 vector2, int index)
    {
        if (vector2 == Vector2.up)
            return upShootingSprites[index];
        else if (vector2 == Vector2.down || vector2 == Vector2.zero)
            return downShootingSprites[index];
        else if (vector2 == Vector2.left)
            return leftShootingSprites[index];
        else if (vector2 == Vector2.right)
            return rightShootingSprites[index];
        else
        { Debug.LogError($"Fatal vector2 {vector2}"); return downShootingSprites[index]; }
    }
}