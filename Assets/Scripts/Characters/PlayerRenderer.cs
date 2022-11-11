using System;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour, IObjectInRoom
{
    [Header("Misc")]
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private HeadSpriteGroup curHeadSpriteGroup;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animation invincibleAnim;
    [SerializeField] private Transform[] muzzles;

    private int animLeftRightID = Animator.StringToHash("LeftRight");
    private int animUpDownID = Animator.StringToHash("UpDown");
    private Sprite preHeadSprite;
    private HeadSpriteGroup preHeadSpriteGroup;
    private GameCoordinate coordinate;


    public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }

    public SpriteRenderer ObjectRenderer => throw new NotImplementedException();

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        ChangeRendererOrder();
    }

    internal void SetMoveAnimator(float x, float y)
    {
        bodyAnimator.SetFloat(animLeftRightID, Mathf.Abs(x));
        bodyAnimator.SetFloat(animUpDownID, Mathf.Abs(y));

        if (x < 0) { bodyRenderer.flipX = true; }
        if (x > 0) { bodyRenderer.flipX = false; }
    }

    internal void SetInvincibleAnimation()
    {
        invincibleAnim.Play();
    }

    internal void SetHeadSpriteGroup(HeadSpriteGroup spriteGroup)
    {
        preHeadSpriteGroup = curHeadSpriteGroup;
        curHeadSpriteGroup = spriteGroup;

        SetHeadSprite(Vector2.down, HeadSpriteGroup.OPEN_EYE_SPRITE_INDEX);
    }

    internal void RevertHeadSpriteGroup()
    {
        curHeadSpriteGroup = preHeadSpriteGroup;

        SetHeadSprite(Vector2.down, HeadSpriteGroup.OPEN_EYE_SPRITE_INDEX);
    }

    internal void SetHeadSprite(Vector2 direction, int index)
    {

        Sprite newSprite = curHeadSpriteGroup.GetSprite(direction, index);
        if (newSprite == headRenderer.sprite) return;

        headRenderer.sprite = newSprite;
    }

    int muzzleSwitch = 0;

    public Vector3 GetTearSpawnPosition(Vector2 vector2)
    {
        muzzleSwitch = 1 - muzzleSwitch;
        if (vector2 == Vector2.up)
            return muzzles[1 - muzzleSwitch].position;
        else if (vector2 == Vector2.down)
            return muzzles[3 - muzzleSwitch].position;
        else if (vector2 == Vector2.left)
            return muzzles[5 - muzzleSwitch].position;
        else if (vector2 == Vector2.right)
            return muzzles[7 - muzzleSwitch].position;
        else
        { Debug.LogError($"Fatal vector2 {vector2}"); return Vector3.zero; }
    }

    public void ChangeRendererOrder()
    {
        bodyRenderer.sortingOrder = (int)(transform.position.y * -5);
        headRenderer.sortingOrder = bodyRenderer.sortingOrder + 1;
    }

    public void ResetObject()
    {
        throw new NotImplementedException();
    }
}
