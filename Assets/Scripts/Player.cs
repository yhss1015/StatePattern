using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isBusy { get; private set; }
    [Header("���� ����")]
    public Vector2[] playerMovement;
    [Header("�̵� ����")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    [Header("��� ����")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public float slidingSpeed;

    [Header("�浹 ����")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;
    

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    // �÷��̾��� ���¸� �����ϴ� ���� �ӽ�
    public PlayerStateMachine stateMachine { get; private set; }

    // �÷��̾��� ���� (��� ����, �̵� ����)
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    
    #endregion

    private void Awake()
    {
        // ���� �ӽ� �ν��Ͻ� ����
        stateMachine = new PlayerStateMachine();

        // �� ���� �ν��Ͻ� ���� (this: �÷��̾� ��ü, stateMachine: ���� �ӽ�, "Idle"/"Move": ���� �̸�)
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState); //idle���·� �ʱ�ȭ
    }



    private void Update()
    {
        stateMachine.currentstate.Update();
        CheckForDashInput();
    }

    
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentstate.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift)&&dashUsageTimer<0)
        {
            dashUsageTimer = dashCooldown;

            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    #region �ӷ�
    public void ZeroVelocity() => rb.linearVelocity = new Vector2(0, 0);


    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region �浹
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion

    #region �ø�
    public void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if(_x>0 && !facingRight)
        {
            Flip();
        }
        else if(_x<0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
}
