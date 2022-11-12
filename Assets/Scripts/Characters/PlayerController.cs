using CustomPhysics2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool controllerEnabled = true;
    private const int MOVE_SPEED_MULTIPLIER = 5;
    private float SHOT_SPEED_MULTIPLIER = 5;

    [SerializeField] private TwoVector3EventChannelSO onEnterRoomEvent;
    [SerializeField] private FloatEventChannelSO onPlayerTearsChanged;
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
    private WaitForFixedFrame waitForFixedFrame;
    private float tearInterval;
    private float shotSpeed;
    private Vector2 tearDirection = Vector2.zero;
    private bool canShoot;
    private float shootTimer;

    private CustomRigidbody2D customRigidbody;
    private Player player;

    public Vector2 PlayerMoveDirection => tempPlayerVelocity;

    public bool ControllerEnabled
    {
        get => controllerEnabled;
        set
        {
            controllerEnabled = value;
            if (!value)
                customRigidbody.velocity = tempPlayerVelocity = Vector2.zero;
        }
    }
    public CustomRigidbody2D CustomRigidbody => customRigidbody;

    private void OnEnable()
    {
        onEnterRoomEvent.OnEventRaised += Refresh;
        onPlayerTearsChanged.OnEventRaised += ChangeTearInterval;
    }

    private void OnDisable()
    {
        onEnterRoomEvent.OnEventRaised -= Refresh;
        onPlayerTearsChanged.OnEventRaised -= ChangeTearInterval;
    }

    void Awake()
    {
        customRigidbody = GetComponent<CustomRigidbody2D>();
        player = GetComponent<Player>();
    }

    void Start()
    {
        moveSpeed *= player.MoveSpeed * MOVE_SPEED_MULTIPLIER;
        shotSpeed = player.ShotSpeed * SHOT_SPEED_MULTIPLIER;

        tearInterval = 1f / player.Tears;
        waitForFixedFrame = new WaitForFixedFrame(Mathf.FloorToInt(player.TearDelay / 2));
    }

    private void Update()
    {
        UpdateSystemControl();
        UpdateGameControl();

        if (!useFixedUpdate)
        {
            UpdateMovement();
            UpdateAnimation();
        }
    }

    private void FixedUpdate()
    {
        FixedUpdateGameControl();
        if (useFixedUpdate)
        {
            UpdateMovement();
            UpdateAnimation();
        }
    }

    private void UpdateMovement()
    {
        if (!controllerEnabled) return;

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
        player.SetMoveAnimator(moveInput.x, moveInput.y);
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            CustomDebugger.Log("Place bomb.");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.UseActiveItem();
        }
    }
    private void FixedUpdateGameControl()
    {
        //canShoot = true;
        tearDirection = Input.GetAxis("Fire1") * Vector2.right;
        if (tearDirection.x == 0) tearDirection += Input.GetAxis("Fire2") * Vector2.up;

        if (tearDirection == Vector2.zero)
        {
            if (canShoot)
            {
                canShoot = false;
                player.SetHeadSprite(tearDirection.normalized, HeadSpriteGroup.OPEN_EYE_SPRITE_INDEX);
            }
        }
        else canShoot = true;
        //StartCoroutine(nameof(FireCoroutine));

        if (shootTimer <= 0f)
        {
            if (canShoot)
            {
                shootTimer = tearInterval;
                ObjectPoolManager.Release(tearPrefab,
                                          player.GetTearSpawnPosition(tearDirection)/*.ToWorldPosition(transform)*/,
                                          Quaternion.identity).GetComponent<Tear>().MoveVelocity = tearDirection * shotSpeed;

                StartCoroutine(nameof(Cor_SetHeadSprite));
            }
            else return;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    IEnumerator Cor_SetHeadSprite()
    {
        player.SetHeadSprite(tearDirection.normalized, HeadSpriteGroup.CLOSE_EYE_SPRITE_INDEX);

        yield return waitForFixedFrame;
        waitForFixedFrame.Reset();

        player.SetHeadSprite(tearDirection.normalized, HeadSpriteGroup.OPEN_EYE_SPRITE_INDEX);
    }

    private void Refresh(Vector3 cameraPosition, Vector3 playerPosition)
    {
        transform.position = playerPosition;
        tempPlayerVelocity = Vector2.zero;
    }

    private void ChangeTearInterval(float tears)
    {
        tearInterval = 1f / tears;

        waitForFixedFrame.ChangeWaitForFrame(Mathf.FloorToInt(tears / 2));
    }
}

public class WaitForFixedFrame : CustomYieldInstruction
{
    private int waitForFrame;
    private float timer;
    public WaitForFixedFrame(int waitForFrame)
    {
        this.waitForFrame = waitForFrame;
        timer = Time.fixedDeltaTime * waitForFrame;
    }

    public override bool keepWaiting
    {
        get
        {
            timer -= Time.deltaTime;
            return timer > 0;
        }
    }
    public override void Reset()
    {
        base.Reset();

        timer = Time.fixedDeltaTime * waitForFrame;
    }

    public void ChangeWaitForFrame(int newFrameCount)
    {
        waitForFrame = newFrameCount;
        Reset();
    }
}
