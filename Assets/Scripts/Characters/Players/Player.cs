using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;

    private PlayerProfileTreeElement playerProfile;
    private MoveController moveController;

    private void OnEnable()
    {
        moveController = GetComponent<MoveController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerProfile = GameMgr.Instance.GetPlayerProfileByID((int)playerCharacter);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
