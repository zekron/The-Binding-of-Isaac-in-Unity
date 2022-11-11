using CustomPhysics2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PickupObject
{
    [SerializeField] private BombSO bombSO;

    private BombSO.BombType bombType;
    private int bombWorth;
    private Coroutine cor_Explosion;
    private Coroutine cor_AutoMove;
    private GameObject target;

    public void SetType(BombSO.BombType type)
    {
        bombType = type;
        objectRenderer.sprite = bombSO.BombSprites[(int)bombType];
        bombWorth = BombSO.BombWorth[(int)bombType];

        if (bombType != BombSO.BombType.MegaTroll && bombType != BombSO.BombType.Troll) return;

        if (cor_Explosion != null)
        {
            StopCoroutine(cor_Explosion);
        }
        cor_Explosion = StartCoroutine(nameof(ExplosionCoroutine));

        return;
        if (bombType == BombSO.BombType.MegaTroll)
        {
            if (target == null) target = FindObjectOfType<Player>().gameObject;

            collisionController.SelfCollider.IsTrigger = true;

            if (cor_AutoMove != null)
            {
                StopCoroutine(cor_AutoMove);
            }
            cor_AutoMove = StartCoroutine(nameof(AutoMoveCoroutine));
        }
    }

    public override void ResetObject()
    {
        base.ResetObject();

        SetType(bombSO.GenerateType());
    }

    protected override void OnPlayerCollect()
    {
        gamePlayer.GetBomb(bombWorth);
        //else refresh context

    }

    protected override void OnPlayerCannotCollect(CollisionInfo2D collisionInfo)
    {
        var direction = (transform.position - gamePlayer.transform.position).normalized;
        customRigidbody.AddForce(direction * 1.5f);
    }
    protected override bool CanPickUp()
    {
        return collisionController.SelfCollider.IsTrigger = bombType == BombSO.BombType.Single || bombType == BombSO.BombType.Double;
    }
    IEnumerator AutoMoveCoroutine()
    {
        Vector3 moveDirection;
        while (gameObject.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;
            transform.Translate(moveDirection * 2.5f * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
