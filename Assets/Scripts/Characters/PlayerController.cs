using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IObjectInRoom
{
    private const int MOVE_SPEED_MULTIPLIER = 5;

    [SerializeField] private TwoVector3EventChannelSO onEnterRoomEvent;
    [SerializeField] private bool useFixedUpdate = false;

    [Header("Move")]
    /// <summary>
    /// 变速因子
    /// </summary>
    [SerializeField, Range(0, 5f)] private float shiftFactor = 3f;
    private bool canMove = false;
    private Vector2 moveInput = Vector2.zero;
    private Vector2 finalMoveDirection;
    private Vector2 tempPlayerVelocity;
    private float moveSpeed = 1f;

    [Header("Shooting")]
    [SerializeField] private GameObject tearPrefab;
    [SerializeField, Range(0.1f, 3f)] private float tearInterval;
    private bool canShoot;
    private float shootTimer;

    [Header("Miscs")]
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private Animator bodyAnimator;

    private CustomRigidbody2D customRigidbody;
    private GameCoordinate coordinate;

    public GameCoordinate Coordinate { get => coordinate; set => coordinate = value; }

    public SpriteRenderer ObjectRenderer => throw new NotImplementedException();

    private void OnEnable()
    {
        onEnterRoomEvent.OnEventRaised += Refresh;
        customRigidbody = GetComponent<CustomRigidbody2D>();
    }

    private void OnDisable()
    {
        onEnterRoomEvent.OnEventRaised -= Refresh;
    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed *= MOVE_SPEED_MULTIPLIER;
    }

    private void Update()
    {
        UpdateSystemControl();

        if (!useFixedUpdate)
        {
            UpdateGameControl();
            UpdateMovement();
            UpdateAnimation();
            ChangeRendererOrder();
        }
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            UpdateGameControl();
            UpdateMovement();
            UpdateAnimation();
            ChangeRendererOrder();
        }
    }

    private void UpdateMovement()
    {
        GenerateMoveDirection();
        if (!canMove && tempPlayerVelocity == Vector2.zero) return;

        //tempPlayerVelocity = finalMoveDirection;

        if (finalMoveDirection == Vector2.zero)
        {
            tempPlayerVelocity = Vector2.Lerp(tempPlayerVelocity, finalMoveDirection, GetMoveDeltaTime() * moveSpeed * shiftFactor);
            if (Mathf.Abs(tempPlayerVelocity.x) <= 1e-2) tempPlayerVelocity.x = 0;
            if (Mathf.Abs(tempPlayerVelocity.y) <= 1e-2) tempPlayerVelocity.y = 0;
        }
        else
            tempPlayerVelocity = finalMoveDirection;

        customRigidbody.velocity = tempPlayerVelocity;
        //transform.Translate(tempPlayerVelocity * GetMoveDeltaTime(), Space.World);
        //transform.position = Viewport.PlayerMoveablePosition(transform.position, paddingX, paddingY);
    }

    private void GenerateMoveDirection()
    {
        moveInput = Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up;
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }

        finalMoveDirection = moveInput * moveSpeed;
        canMove = finalMoveDirection != Vector2.zero;
    }

    private float GetMoveDeltaTime()
    {
        return useFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime;
    }

    private void UpdateAnimation()
    {
        if (moveInput.x < 0) { bodyRenderer.flipX = true; }
        if (moveInput.x > 0) { bodyRenderer.flipX = false; }
        bodyAnimator.SetFloat("UpDown", Mathf.Abs(moveInput.y));
        bodyAnimator.SetFloat("LeftRight", Mathf.Abs(moveInput.x));
    }

    private void UpdateSystemControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CustomDebugger.Log(string.Format("Key {0} pressed.", KeyCode.Escape));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateGameControl()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            canShoot = true;
            if (canShoot)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer >= tearInterval) shootTimer = 0f;
            }
            var tear = ObjectPoolManager.Release(tearPrefab,
                                                 headRenderer.transform.position,
                                                 Quaternion.identity).GetComponent<Tear>();
            tear.MoveVelocity = Vector2.left;
        }
    }

    private void Refresh(Vector3 cameraPosition, Vector3 playerPosition)
    {
        transform.position = playerPosition;
        tempPlayerVelocity = Vector2.zero;
    }

    public void ChangeRendererOrder()
    {
        bodyRenderer.sortingOrder = (int)(transform.position.y * -5);
        headRenderer.sortingOrder = bodyRenderer.sortingOrder + 1;
    }

    public void ResetObject()
    {

    }
}
