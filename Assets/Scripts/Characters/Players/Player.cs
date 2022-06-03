using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;

    private PlayerProfileTreeElement playerProfile;
    private PlayerController moveController;

    #region Current player status
    private float tearAddition = 0;
    private float shotSpeedAddition = 0;
    private float moveSpeedAddition = 0;
    #endregion

    public float Tears { get => playerProfile.GetTears(PlayerProfileTreeElement.GetTearDelay(tearAddition)); }
    public float ShotSpeed { get => playerProfile.ShotSpeed + shotSpeedAddition; }
    public float MoveSpeed { get => playerProfile.BaseMoveSpeed + moveSpeedAddition; }

    private void Awake()
    {
        Initialize((int)playerCharacter);
    }
    private void OnEnable()
    {
        moveController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize(int id)
    {
        playerProfile = GameMgr.Instance.GetPlayerProfileByID(id);
    }
}
