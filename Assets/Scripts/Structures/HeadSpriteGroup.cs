using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Head Sprite Group", menuName = "Scriptable Object/Sprites/Head Sprite Group")]
public class HeadSpriteGroup : ScriptableObject
{
    [SerializeField] private Sprite[] upShootingSprites = new Sprite[2];
    [SerializeField] private Sprite[] downShootingSprites = new Sprite[2];
    [SerializeField] private Sprite[] leftShootingSprites = new Sprite[2];
    [SerializeField] private Sprite[] rightShootingSprites = new Sprite[2];

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

#if UNITY_EDITOR
    public void InitializeGroup(Sprite sprite, int index)
    {
        if (index < 1 || index > 9) { Debug.LogError($"Fatal index input => {index}"); return; }

        if (index < 3)
            downShootingSprites[(index - 1) % 2] = sprite;
        else if (index < 5)
            leftShootingSprites[(index - 1) % 2] = sprite;
        else if (index < 7)
            upShootingSprites[(index - 1) % 2] = sprite;
        else
            rightShootingSprites[(index - 1) % 2] = sprite;
    }
#endif
}