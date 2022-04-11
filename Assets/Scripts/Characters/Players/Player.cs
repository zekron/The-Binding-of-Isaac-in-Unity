using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class Player : MonoBehaviour
{
    //[SerializeField] private int playerCharacter;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;

    private PlayerProfileTreeElement playerProfile;
    private MoveController moveController;

    private void OnEnable()
    {
        moveController = GetComponent<MoveController>();
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
        GameMgr.Instance.GetPlayerProfileByID(id);
    }
}
